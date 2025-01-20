using GunExtreme.Entities;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Players;
using ZombieGame.GameController.EntityManager;
using ZombieGame.GameController.GameTimer;
using System.Reflection;
using Microsoft.VisualBasic.Logging;
using Logger;
using SpatialStructures.Notifier;
using ZombieGame.Entities.Enemies;

namespace ZombieGame.GameController
{
    /// <summary>
    /// Main game controller that handles the game's core logic, state, and events.
    /// </summary>
    public static class GameManager
    {
        // Quadtree for managing game object collisions.
        private static Quadtree<ICollidable> _gameQuadtree;

        // Main game form.
        private static Game _gameForm;

        // Player instance.
        private static Player _player;

        // Game timer controller.
        private static GameTimerController _gameTimerController;

        // Game entities manager.
        private static ManagerGameEntities _entityManager;

        // Keyboard input manager.
        private static KeyManager _keyManager;

        // Logger manager for recording events.
        private static LoggerManager _log;

        // Indicates whether the game is paused.
        public static bool IsPaused { get; private set; }

        // Indicates whether the game is running.
        public static bool GameIsRunning { get; private set; } = false;

        // Indicates whether to show entity hitboxes.
        public static bool ShowHitbox { get; private set; } = false;

        /// <summary>
        /// Gets the game window's size.
        /// </summary>
        public static Size WindowsSize
        {
            get
            {
                return _gameForm.Size;
            }
        }

        /// <summary>
        /// Initializes the main game components, including the form, player, timer, entity manager, and other essentials.
        /// </summary>
        /// <param name="gameForm">The main game form.</param>
        public static void InitializeComponent(Game gameForm)
        {
            _gameForm = gameForm;
            GenerateLog();

            Rectangle formSize = new(0, 0, _gameForm.Width, _gameForm.Height);

            var quadtreNotification = new ENotificationType[]
            {
                ENotificationType.OnInsert
            };

            _gameQuadtree = new Quadtree<ICollidable>(formSize, notificationTypes: quadtreNotification, maxLvls: 2);

            _entityManager = new();
            _player = _entityManager.GenerateNewPlayer("Mariano");

            _keyManager = new(_player);

            CollisionHandler.InitializeCollisionGame(_player, _gameQuadtree);

            int gameTicks = 16;
            _gameTimerController = new GameTimerController(gameTicks, UpdateGame);

            _gameQuadtree.OnReDraw += ReloadUI;
            _gameQuadtree.OnChanged += static (string message) => { _log.LogInfo(message); };

            _gameForm.Shown += (s, e) => ResumeGame();
            GameIsRunning = true;
        }

        /// <summary>
        /// Updates the game's state, including entities and collisions.
        /// </summary>
        private static void UpdateGame()
        {
            _entityManager.UpdateEntities();
            CollisionHandler.CheckCollisions();
        }

        /// <summary>
        /// Creates a log file in the project's directory to record events.
        /// </summary>
        private static void GenerateLog()
        {
            string projectDirectory = Directory.GetCurrentDirectory();

            string newDirectoryPath = Path.Combine(projectDirectory, "OutputLog");

            if (!Directory.Exists(newDirectoryPath))
            {
                Directory.CreateDirectory(newDirectoryPath);
            }

            _log = new(newDirectoryPath);
        }

        /// <summary>
        /// Resumes the game, restarting timers and hiding menu buttons.
        /// </summary>
        public static void ResumeGame()
        {
            IsPaused = false;
            _gameTimerController.Start();
            _entityManager.ResumeSpawnTimer();
            _gameForm.HideMenuButtons();
        }

        /// <summary>
        /// Stops the game, pausing timers and the entity manager.
        /// </summary>
        public static void StopGame()
        {
            IsPaused = true;
            _gameTimerController.Stop();
            _entityManager.StopSpawnTimer();
        }

        /// <summary>
        /// Restarts the game by resetting the player's state, entities, and UI.
        /// </summary>
        public static void RestartGame()
        {
            GameIsRunning = false;

            _gameForm.HideMenuButtons();

            _gameQuadtree.Clear();

            _entityManager.ResetManagerAsync();
            _player.Reset(new(0, 0));
            AddEntity(_player);

            ReloadUI();
            ResumeGame();
            _gameForm.Focus();

            GameIsRunning = true;
        }

        /// <summary>
        /// Handles key-down events.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Key event arguments.</param>
        public static void Game_KeyDown(object? sender, KeyEventArgs e)
        {
            _keyManager.KeyDown(e.KeyCode);
        }

        /// <summary>
        /// Handles key-up events.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Key event arguments.</param>
        public static void Game_KeyUp(object? sender, KeyEventArgs e)
        {
            _keyManager.KeyUp(e.KeyCode);
        }

        /// <summary>
        /// Refreshes the game's user interface.
        /// </summary>
        public static void ReloadUI()
        {
            _gameForm.Invalidate();
        }

        /// <summary>
        /// Gets the current mouse position in the form.
        /// </summary>
        /// <returns>Mouse position.</returns>
        public static Point GetMousePosition()
        {
            return _gameForm.MousePosition;
        }

        /// <summary>
        /// Adds an entity to the game, activating it and adding it to the Quadtree and form.
        /// </summary>
        /// <param name="obj">Entity to add.</param>
        public static void AddEntity(AbstractEntity obj)
        {
            var entity = obj as ICollidable ?? throw new Exception("Entity is not a ICollidable object");
            entity.IsActive = true;

            _gameQuadtree.Insert(entity, entity.Area);
            _gameForm.Controls.Add(obj.PictureBox);
        }

        /// <summary>
        /// Removes an entity from the game, deactivating it and removing it from the Quadtree and form.
        /// </summary>
        /// <param name="obj">Entity to remove.</param>
        public static void RemoveEntity(AbstractEntity obj)
        {
            var entity = obj as ICollidable ?? throw new Exception("Entity is not a ICollidable object");
            entity.IsActive = false;

            _gameQuadtree.Remove(entity);
            _gameForm.Controls.Remove(obj.PictureBox);
        }

        /// <summary>
        /// Resizes the Quadtree to the given size.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        public static void ReSizeQuadtree(int width, int height)
        {
            _gameQuadtree.Resize(width, height);
        }

        /// <summary>
        /// Draws Quadtree boundaries and hitboxes if enabled.
        /// </summary>
        /// <param name="e">Drawing event arguments.</param>
        public static void DrawQuadtreeBounds(PaintEventArgs e)
        {
            if (!ShowHitbox) return;

            using Pen pen = new Pen(Color.Red, 1);

            var bounds = _gameQuadtree?.GetAllBounds();

            foreach (var rect in bounds)
            {
                e.Graphics.DrawRectangle(pen, rect);
            }

            e.Graphics.DrawRectangle(pen, _player.ViewArea);

            pen.Dispose();
        }

        /// <summary>
        /// Ends the game and shows the main menu.
        /// </summary>
        public static void GameOver()
        {
            StopGame();
            _gameForm.ShowMenuButtons();
            _gameForm.ShowGameOver();
        }

        /// <summary>
        /// Toggles the visibility of entity hitboxes.
        /// </summary>
        public static void ShowHitboxes()
        {
            ShowHitbox = !ShowHitbox;
            ReloadUI();
        }

        /// <summary>
        /// Decreases the player's health points by the specified amount.
        /// </summary>
        /// <param name="amount">Amount to decrease.</param>
        public static void SubtractPlayerHP(int amount)
        {
            _gameForm.WritePlayerHp(amount);
        }

        /// <summary>
        /// Increments the player's kill count by the specified amount.
        /// </summary>
        /// <param name="amount">Amount to increment.</param>
        public static void SumPlayerKill(int amount)
        {
            _gameForm.SumPlayerKill(amount);
        }

        /// <summary>
        /// Opens or closes the main menu depending on the current game state.
        /// </summary>
        public static void OpenMenu()
        {
            if (IsPaused)
            {
                _gameForm.HideMenuButtons();
                ResumeGame();
            }
            else
            {
                _gameForm.ShowMenuButtons();
                StopGame();
            }
        }
    }
}

using GunExtreme.Entities;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Ammunition;
using ZombieGame.Entities.Enemies;
using ZombieGame.Entities.Players;
using ZombieGame.GameController.GameTimer;

namespace ZombieGame.GameController.EntityManager
{
    public class ManagerGameEntities : IManagerGameEntities
    {
        private Player _player;
        private readonly ObjectPool<AbstractEntity> _bulletPool;
        private readonly ObjectPool<AbstractEntity> _zombiesPool;
        private readonly GameTimerController _spawnRatio;

        // Define a constant for max number of active enemies at once
        private const int MaxEnemiesActive = 3;

        // Constructor initializing the pools and spawn timer
        public ManagerGameEntities()
        {
            _bulletPool = new ObjectPool<AbstractEntity>();
            _zombiesPool = new ObjectPool<AbstractEntity>();

            double spawnInterval = 5; // Spawn every 5 seconds
            _spawnRatio = new GameTimerController(spawnInterval, SpawnEnemyAsync);
        }

        /// <summary>
        /// Resets all active entities (bullets, zombies) and releases them back to their respective pools.
        /// </summary>
        public async Task ResetManagerAsync()
        {
            var allEntities = new List<AbstractEntity>();
            allEntities.AddRange(_bulletPool.Objects);
            allEntities.AddRange(_zombiesPool.Objects);

            foreach (var entity in allEntities)
            {
                await entity.Reset(new Vector2(0, 0));
                if (entity.IsActive)
                {
                    ReleaseEntity(entity); // Release inactive entities back to the pool
                }
            }
        }

        /// <summary>
        /// Starts the spawn timer to generate zombies periodically.
        /// </summary>
        public void ResumeSpawnTimer()
        {
            _spawnRatio.Start();
        }

        /// <summary>
        /// Stops the spawn timer, halting zombie generation.
        /// </summary>
        public void StopSpawnTimer()
        {
            _spawnRatio.Stop();
        }

        /// <summary>
        /// Releases an entity back to its corresponding pool (bullet pool, zombie pool, or ends game if player is released).
        /// </summary>
        /// <param name="entity">The entity to release back to its pool.</param>
        public void ReleaseEntity(AbstractEntity entity)
        {
            if (!entity.IsActive) return;

            switch (entity)
            {
                case IAmmunition:
                    _bulletPool.ReleaseObject(entity); // Release bullets back to the pool
                    break;
                case IEnemy:
                    _zombiesPool.ReleaseObject(entity); // Release zombies back to the pool
                    break;
                case IPlayer:
                    GameManager.GameOver(); // End game if the player is released
                    return;
                default:
                    throw new InvalidOperationException("Invalid object type");
            }

            GameManager.RemoveEntity(entity);
            entity.IsActive = false;
        }

        /// <summary>
        /// Updates all active entities (bullets, zombies) and tracks how many entities were updated.
        /// </summary>
        /// <returns>A tuple containing the count of updated entities and the list of updated entities.</returns>
        public (int Count, List<AbstractEntity> UpdatedEntities) UpdateEntities()
        {
            var entitiesToUpdate = new List<AbstractEntity>();
            entitiesToUpdate.AddRange(_bulletPool.ObjectsActive);
            entitiesToUpdate.AddRange(_zombiesPool.ObjectsActive);

            var updatedEntities = new List<AbstractEntity>();
            int count = 0;

            foreach (var entity in entitiesToUpdate)
            {
                if (entity.HasChanged)
                {
                    entity.Update();
                    updatedEntities.Add(entity); // Track updated entities
                    count++;
                }
            }

            return (count, updatedEntities); // Return the number of updated entities and the updated entities themselves
        }

        /// <summary>
        /// Gets the current position of the player.
        /// </summary>
        /// <returns>The position of the player.</returns>
        public Vector2 GetPlayerPosition()
        {
            return _player.Position;
        }

        /// <summary>
        /// Gets the current position of the mouse in the game.
        /// </summary>
        /// <returns>The mouse position as a Point.</returns>
        public Point GetMousePosition()
        {
            return GameManager.GetMousePosition();
        }

        /// <summary>
        /// Spawns a new zombie if the number of active zombies is less than the maximum allowed.
        /// </summary>
        private async void SpawnEnemyAsync()
        {
            if (_zombiesPool.ObjectsActive.Count >= MaxEnemiesActive)
                return;

            Vector2 enemyPosition = GenerateRandomPosition();

            // Only spawn if the zombie is not in the player's view area
            if (!_player.ViewArea.Contains((int)enemyPosition.X, (int)enemyPosition.Y))
            {
                await GenerateNewZombieAsync("default", (int)enemyPosition.X, (int)enemyPosition.Y);
            }
        }

        /// <summary>
        /// Generates a random position for spawning an enemy within the game area.
        /// </summary>
        /// <returns>A random position as a Vector2.</returns>
        private static Vector2 GenerateRandomPosition()
        {
            var gameSize = GameManager.WindowsSize;
            var random = new Random();

            float x = random.Next(0, gameSize.Width);
            float y = random.Next(0, gameSize.Height);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Creates a new player with the specified name and initializes it.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <returns>The newly created player.</returns>
        /// <exception cref="ArgumentException">Thrown when the player name is null or empty.</exception>
        public Player GenerateNewPlayer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player name cannot be null or empty", nameof(name));

            _player = new Player(
                name,
                10,
                0,
                new Rectangle(10, 10, 50, 50),
                5,
                new PictureBox
                {
                    Width = 50,
                    Height = 50,
                    BackColor = Color.Blue,
                    Location = new Point(10, 10)
                },
                this
            );

            GameManager.AddEntity(_player);
            return _player;
        }

        /// <summary>
        /// Generates a new zombie at the specified position.
        /// </summary>
        /// <param name="type">The type of the zombie (e.g., default, special).</param>
        /// <param name="x">The x-coordinate for the zombie's position.</param>
        /// <param name="y">The y-coordinate for the zombie's position.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GenerateNewZombieAsync(string type, int x, int y)
        {
            var zombie = (Zombie)_zombiesPool.GetObject(() => CreateZombie(type, x, y));

            if (zombie.IsFromPool)
            {
                await zombie.Reset(new Vector2(x, y)); // Reset position if it's from the pool
            }

            GameManager.AddEntity(zombie);
        }

        /// <summary>
        /// Helper method to create a new zombie entity.
        /// </summary>
        /// <param name="type">The type of the zombie.</param>
        /// <param name="x">The x-coordinate for the zombie's position.</param>
        /// <param name="y">The y-coordinate for the zombie's position.</param>
        /// <returns>A newly created zombie entity.</returns>
        private Zombie CreateZombie(string type, int x, int y)
        {
            return new Zombie(
                type,
                1,
                10,
                new Rectangle(x, y, 50, 50),
                3,
                new PictureBox
                {
                    Width = 50,
                    Height = 50,
                    BackColor = Color.Green,
                    Location = new Point(x, y)
                },
                this
            );
        }

        /// <summary>
        /// Generates a new bullet entity and returns it.
        /// </summary>
        /// <returns>A new bullet entity.</returns>
        public Bullet GenerateNewBullet()
        {
            return (Bullet)_bulletPool.GetObject(() => new Bullet(
                4,
                0,
                new Rectangle(0, 0, 25, 25),
                5,
                new PictureBox
                {
                    Width = 25,
                    Height = 25,
                    BackColor = Color.Yellow
                },
                this
            ));
        }
    }
}

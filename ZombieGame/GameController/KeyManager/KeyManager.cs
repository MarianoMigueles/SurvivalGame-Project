using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Players;

namespace ZombieGame.GameController
{
    /// <summary>
    /// Manages key bindings, key states, and player actions in response to user input.
    /// </summary>
    internal class KeyManager
    {
        private readonly Player _player;

        /// <summary>
        /// Stores the current state (pressed/released) of keys.
        /// </summary>
        private readonly Dictionary<Keys, bool> _keyStates = new();

        /// <summary>
        /// Stores the mapping of actions to their respective keys.
        /// </summary>
        private readonly Dictionary<string, Keys> _keyBindings = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyManager"/> class with default key bindings.
        /// </summary>
        /// <param name="player">The player instance to control.</param>
        public KeyManager(Player player)
        {
            _player = player;

            // Default key bindings
            _keyBindings["MoveLeft"] = Keys.A;
            _keyBindings["MoveRight"] = Keys.D;
            _keyBindings["MoveUp"] = Keys.W;
            _keyBindings["MoveDown"] = Keys.S;
            _keyBindings["Fire"] = Keys.Space;
            _keyBindings["Menu"] = Keys.Escape;
        }

        /// <summary>
        /// Updates the key binding for a specified action.
        /// </summary>
        /// <param name="action">The name of the action to update (e.g., "MoveLeft").</param>
        /// <param name="newKey">The new key to bind to the action.</param>
        public void SetKeysBinding(string action, Keys newKey)
        {
            if (_keyBindings.ContainsKey(action))
            {
                _keyBindings[action] = newKey;
            }
        }

        /// <summary>
        /// Handles the key down event and executes the corresponding action.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public async void KeyDown(Keys key)
        {
            foreach (var binding in _keyBindings)
            {
                if (binding.Value == key)
                {
                    _keyStates[key] = true;
                    await HandleKeyPressAsync(binding.Key);
                }
            }
        }

        /// <summary>
        /// Handles the key up event and updates the key state.
        /// </summary>
        /// <param name="key">The key that was released.</param>
        public void KeyUp(Keys key)
        {
            foreach (var binding in _keyBindings)
            {
                if (binding.Value == key)
                {
                    _keyStates[key] = false;
                }
            }
        }

        /// <summary>
        /// Asynchronously processes the action associated with a key press.
        /// </summary>
        /// <param name="action">The name of the action to process.</param>
        private async Task HandleKeyPressAsync(string action)
        {
            if (action == "Menu")
            {
                OpenMenu();
            }

            if (GameManager.IsPaused) return;

            while (_keyStates[_keyBindings[action]])
            {
                switch (action)
                {
                    case "MoveLeft":
                        MoveLeft();
                        break;
                    case "MoveRight":
                        MoveRight();
                        break;
                    case "MoveUp":
                        MoveUp();
                        break;
                    case "MoveDown":
                        MoveDown();
                        break;
                    case "Fire":
                        Fire();
                        break;
                }

                await Task.Delay(50); // Introduce a delay to manage key press frequency
            }
        }

        /// <summary>
        /// Moves the player down.
        /// </summary>
        private void MoveDown()
        {
            UpdatePosition(0, 1);
        }

        /// <summary>
        /// Moves the player up.
        /// </summary>
        private void MoveUp()
        {
            UpdatePosition(0, -1);
        }

        /// <summary>
        /// Moves the player to the right.
        /// </summary>
        private void MoveRight()
        {
            UpdatePosition(1, 0);
        }

        /// <summary>
        /// Moves the player to the left.
        /// </summary>
        private void MoveLeft()
        {
            UpdatePosition(-1, 0);
        }

        /// <summary>
        /// Fires the player's weapon.
        /// </summary>
        private void Fire()
        {
            _player.Shoot();
        }

        /// <summary>
        /// Opens the game menu.
        /// </summary>
        private void OpenMenu()
        {
            GameManager.OpenMenu();
        }

        /// <summary>
        /// Updates the player's position based on velocity and direction.
        /// </summary>
        /// <param name="x">The x-direction to move (e.g., -1 for left, 1 for right).</param>
        /// <param name="y">The y-direction to move (e.g., -1 for up, 1 for down).</param>
        private void UpdatePosition(int x, int y)
        {
            _player.Position += new Vector2(x, y) * _player.Velocity;
        }
    }
}

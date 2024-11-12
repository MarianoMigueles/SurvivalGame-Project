using GunExtreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.GameController
{
    internal class KeyManager
    {

        private readonly Player _player;
        private readonly Game _gameForm;

        private readonly Dictionary<Keys, bool> _keyStates = [];

        private readonly Dictionary<string, Keys> _keyBindings = [];

        public KeyManager(Player player, Game gameForm)
        {
            _player = player;
            _gameForm = gameForm;

            _keyBindings["MoveLeft"] = Keys.A;
            _keyBindings["MoveRight"] = Keys.D;
            _keyBindings["MoveUp"] = Keys.W;
            _keyBindings["MoveDown"] = Keys.S;
            _keyBindings["Fire"] = Keys.Space;
        }

        public void SetKeysBinding(string action, Keys newKey)
        {
            if (_keyBindings.ContainsKey(action))
            {
                _keyBindings[action] = newKey;
            }
        }

        public void KeyDown(Keys key)
        {
            foreach (var binding in _keyBindings)
            {
                if (binding.Value == key)
                {
                    _keyStates[key] = true;
                    HandleKeyPressAsync(binding.Key);
                }
            }
        }

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

        private async Task HandleKeyPressAsync(string action)
        {
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
                await Task.Delay(50); //Thread delay
            }
        }

        private void MoveDown()
        {
            _player.PositionY += _player.Velocity; 
        }

        private void MoveUp()
        {
            _player.PositionY -= _player.Velocity;
        }

        private void MoveRight()
        {
            _player.PositionX += _player.Velocity;
        }

        private void MoveLeft()
        {
            _player.PositionX -= _player.Velocity;
        }
        private void Fire()
        {
            _player.Shoot();
        }
    }
}

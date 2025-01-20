using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GunExtreme.Entities;
using ZombieGame.Entities.Ammunition;
using ZombieGame.Entities.Weapons;
using ZombieGame.GameController;
using ZombieGame.GameController.EntityManager;

namespace ZombieGame.Entities.Players
{
    /// <summary>
    /// Represents a player entity in the game.
    /// </summary>
    public class Player : AbstractEntity, IPlayer
    {
        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        private int _Health;

        /// <summary>
        /// Gets or sets the player's health. 
        /// If health drops to 0, the player disappears from the game.
        /// </summary>
        public int Health
        {
            get => _Health;
            set
            {
                _Health = value;
                GameManager.SubtractPlayerHP(value);

                if (_Health <= 0)
                {
                    Disappear();
                    _Health = 0;
                }
            }
        }

        private int _murders;

        /// <summary>
        /// Gets or sets the number of kills (murders) the player has achieved.
        /// Updates the game manager with the current kill count.
        /// </summary>
        public int Murders
        {
            get => _murders;
            set
            {
                _murders = value;
                GameManager.SumPlayerKill(value);
            }
        }

        /// <summary>
        /// Defines the player's field of view area for detecting nearby entities.
        /// </summary>
        public Rectangle ViewArea { get; set; }

        private readonly Pistol _Weapon;
        private bool _IsRecharging = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">Name of the player.</param>
        /// <param name="health">Initial health of the player.</param>
        /// <param name="id">Unique identifier for the player.</param>
        /// <param name="area">Bounding area of the player.</param>
        /// <param name="velocity">Movement speed of the player.</param>
        /// <param name="pictureBox">UI representation of the player.</param>
        /// <param name="entintyGenerator">Entity manager to handle game entities.</param>
        public Player(string name, int health, int id, Rectangle area, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
            : base(area, pictureBox, velocity, entintyGenerator)
        {
            Name = name;
            Health = health;

            // Define player's view area based on its initial size.
            ViewArea = new Rectangle((area.X * 2) * -1, (area.Y * 2) * -1, area.Width * 4, area.Height * 4);

            // Initialize the player's weapon.
            _Weapon = new Pistol("pistol", "hand", 10, 500);

            // Begin initial weapon recharge process.
            _ = RechargeAsync();
        }

        /// <summary>
        /// Executes the player's shoot action.
        /// </summary>
        public async void Shoot()
        {
            if (!_IsRecharging)
            {
                if (!_Weapon.CanShoot) return;

                if (_Weapon.Bullets.Count > 0)
                {
                    Point mousePosition = _entintyManager.GetMousePosition();

                    _Weapon.CanShoot = false;

                    Point playerLocation = new Point((int)Position.X, (int)Position.Y);

                    Bullet bullet = _Weapon.Bullets[0];
                    _Weapon.Bullets.RemoveAt(0);

                    bullet.Travel(playerLocation, mousePosition);

                    await Task.Delay(_Weapon.FireRateDelay);
                    _Weapon.CanShoot = true;
                }
                else
                {
                    await RechargeAsync();
                }
            }
        }

        /// <summary>
        /// Inflicts damage to the player and updates their health.
        /// Displays a visual effect to indicate damage taken.
        /// </summary>
        /// <param name="damage">Amount of damage to be taken.</param>
        public async void TakeDamage(int damage)
        {
            Health -= damage;

            // Temporary visual indicator of damage.
            PictureBox.BackColor = Color.Red;
            await Task.Delay(500);
            PictureBox.BackColor = Color.Blue;
        }

        /// <summary>
        /// Recharges the player's weapon with bullets asynchronously.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task RechargeAsync()
        {
            _IsRecharging = true;

            // Simulate recharge delay.
            await Task.Delay(2000);

            for (int i = 0; i < _Weapon.AmountBullets; i++)
            {
                Bullet newBullet = _entintyManager.GenerateNewBullet();
                _Weapon.Bullets.Add(newBullet);
            }

            _IsRecharging = false;
        }

        /// <summary>
        /// Resets the player's state to its initial values.
        /// </summary>
        protected override void OnReset()
        {
            Health = 10;
            Murders = 0;
            _ = RechargeAsync();
        }

        /// <summary>
        /// Updates the player's position and recalculates their field of view.
        /// </summary>
        /// <param name="newValue">New position value.</param>
        protected override void OnPositionChange(Vector2 newValue)
        {
            base.OnPositionChange(newValue);

            int playerX = (int)newValue.X;
            int playerY = (int)newValue.Y;
            int playerWidth = Area.Width / 2;
            int playerHeight = Area.Height / 2;

            int visionWidth = ViewArea.Width;
            int visionHeight = ViewArea.Height;

            int visionX = playerX - (visionWidth / 2) + playerWidth;
            int visionY = playerY - (visionHeight / 2) + playerHeight;

            ViewArea = new Rectangle(visionX, visionY, visionWidth, visionHeight);

            if (GameManager.ShowHitbox) GameManager.ReloadUI();
        }
    }
}


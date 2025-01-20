using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GunExtreme.Entities;
using ZombieGame.Entities.Players;
using ZombieGame.GameController.EntityManager;
using Utilities.DirectionalUtilities;
using ZombieGame.GameController;

namespace ZombieGame.Entities.Enemies
{
    /// <summary>
    /// Represents a zombie entity in the game. Zombies can attack players, take damage, and chase the player.
    /// </summary>
    public class Zombie : AbstractEntity, IEnemy
    {
        /// <summary>
        /// Gets or sets the type of the zombie (e.g., "Walker", "Runner").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage the zombie inflicts when attacking.
        /// </summary>
        public int Damage { get; set; }

        private int _Health;

        /// <summary>
        /// Gets or sets the health of the zombie. 
        /// If health reaches 0, the zombie disappears.
        /// </summary>
        public int Health
        {
            get => _Health;
            set
            {
                _Health = Math.Max(0, value);
                if (_Health == 0)
                {
                    base.Disappear();
                }
            }
        }

        /// <summary>
        /// Indicates whether the zombie is currently attacking.
        /// </summary>
        public bool IsAttacking { get; set; }

        private readonly SemaphoreSlim _attackLock = new(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Zombie"/> class.
        /// </summary>
        /// <param name="type">The type of the zombie.</param>
        /// <param name="damage">The amount of damage the zombie inflicts.</param>
        /// <param name="health">The initial health of the zombie.</param>
        /// <param name="area">The area (bounding box) representing the zombie's position and size.</param>
        /// <param name="velocity">The movement speed of the zombie.</param>
        /// <param name="pictureBox">The visual representation of the zombie.</param>
        /// <param name="entintyGenerator">The manager responsible for handling game entities.</param>
        public Zombie(string type, int damage, int health, Rectangle area, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
            : base(area, pictureBox, velocity, entintyGenerator)
        {
            Type = type;
            Health = health;
            Damage = damage;
            PictureBox.BackColor = Color.Green; // Default color for a zombie
        }

        /// <summary>
        /// Makes the zombie attack the specified player, dealing damage.
        /// </summary>
        /// <param name="player">The player to attack.</param>
        public async void Atack(IPlayer player)
        {
            if (IsAttacking) return;
            await _attackLock.WaitAsync();
            try
            {
                IsAttacking = true;
                player.TakeDamage(Damage);
                await Task.Delay(1000);
            }
            finally
            {
                IsAttacking = false;
                _attackLock.Release();
            }
        }

        /// <summary>
        /// Reduces the zombie's health when it takes damage.
        /// Changes the zombie's color temporarily to indicate damage.
        /// </summary>
        /// <param name="damage">The amount of damage to inflict.</param>
        public async void TakeDamage(int damage)
        {
            Health -= damage;
            PictureBox.BackColor = Color.Violet;
            await Task.Delay(500);
            PictureBox.BackColor = Color.Green;
        }

        /// <summary>
        /// Updates the zombie's state, including chasing the player and moving.
        /// </summary>
        public override void Update()
        {
            Chase();
            base.Update();
        }

        /// <summary>
        /// Causes the zombie to chase the player's position.
        /// </summary>
        public void Chase()
        {
            var direction = _entintyManager.GetPlayerPosition();
            CalculateDirection(direction);
        }

        /// <summary>
        /// Calculates the direction the zombie should move toward a destination.
        /// </summary>
        /// <param name="destination">The target position to move toward.</param>
        public void CalculateDirection(Vector2 destination)
        {
            if (Vector2.Distance(Position, destination) < 5)
            {
                Direction = Vector2.Zero;
            }
            else
            {
                Direction = DirectionHelper.CalculateDirection(Position, destination);
            }
        }

        /// <summary>
        /// Resets the zombie's state to its default values.
        /// </summary>
        protected override void OnReset()
        {
            base.OnReset();
            Health = 10;
            Direction = Vector2.Zero;
            IsAttacking = false;
            PictureBox.BackColor = Color.Green;
        }
    }
}



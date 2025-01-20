using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GunExtreme.Entities;
using ZombieGame.Entities.Enemies;
using ZombieGame.Entities.Players;
using ZombieGame.GameController.EntityManager;
using ZombieGame.GameController.ScreenBoundary;
using ZombieGame.GameController;
using Utilities.DirectionalUtilities;
using static System.Windows.Forms.MonthCalendar;

namespace ZombieGame.Entities.Ammunition
{
    /// <summary>
    /// Represents a bullet entity in the game. Bullets can travel toward a target and deal damage to entities.
    /// </summary>
    public class Bullet : AbstractEntity, IAmmunition
    {
        /// <summary>
        /// Gets or sets the amount of damage the bullet inflicts on impact.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bullet"/> class.
        /// </summary>
        /// <param name="damage">The amount of damage the bullet inflicts.</param>
        /// <param name="id">The unique identifier for the bullet.</param>
        /// <param name="area">The area (bounding box) representing the bullet's position and size.</param>
        /// <param name="velocity">The speed at which the bullet travels.</param>
        /// <param name="pictureBox">The visual representation of the bullet.</param>
        /// <param name="entintyGenerator">The manager responsible for handling game entities.</param>
        public Bullet(int damage, int id, Rectangle area, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
            : base(area, pictureBox, velocity, entintyGenerator)
        {
            Damage = damage;
        }

        /// <summary>
        /// Launches the bullet from a starting point toward a target point.
        /// </summary>
        /// <param name="start">The starting position of the bullet.</param>
        /// <param name="target">The target position the bullet is aiming for.</param>
        public void Travel(Point start, Point target)
        {
            Position = new Vector2(start.X, start.Y);
            GameManager.AddEntity(this);
            CalculateDirection(new Vector2(target.X, target.Y));
        }

        /// <summary>
        /// Resets the bullet's state, setting its direction to zero.
        /// </summary>
        protected override void OnReset()
        {
            Direction = Vector2.Zero;
        }

        /// <summary>
        /// Handles the behavior when the bullet can no longer change its position (e.g., collision).
        /// The bullet will disappear.
        /// </summary>
        protected override void OnPositionCanNotChange()
        {
            base.Disappear();
        }

        /// <summary>
        /// Calculates the direction the bullet should travel toward a destination.
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
    }
}


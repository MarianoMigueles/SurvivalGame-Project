using ZombieGame;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities;
using ZombieGame.GameController;
using ZombieGame.GameController.EntityManager;
using ZombieGame.GameController.ScreenBoundary;
using ZombieGame;

namespace GunExtreme.Entities
{
    /// <summary>
    /// Represents an abstract base class for all game entities. 
    /// Implements basic properties and behaviors such as movement, position updates, and event handling.
    /// </summary>
    public abstract class AbstractEntity : IIdentificable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEntity"/> class.
        /// </summary>
        /// <param name="area">The area (bounding box) occupied by the entity.</param>
        /// <param name="pictureBox">The visual representation of the entity.</param>
        /// <param name="velocity">The movement speed of the entity.</param>
        /// <param name="entintyGenerator">The manager responsible for controlling entities in the game.</param>
        public AbstractEntity(Rectangle area, PictureBox pictureBox, int velocity, IManagerGameEntities entintyGenerator)
        {
            Area = area;
            PictureBox = pictureBox;
            Velocity = velocity;
            _entintyManager = entintyGenerator;

            Update(); // Ensure initial update to sync visuals and position
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is from a reusable object pool.
        /// </summary>
        public bool IsFromPool { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether the entity has been updated since its last state.
        /// </summary>
        public bool HasChanged { get; private set; } = false;

        /// <summary>
        /// Event triggered when the area of the entity changes.
        /// </summary>
        public event EventHandler OnAreaChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is being reinserted into active gameplay.
        /// </summary>
        public bool IsReinserting { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is currently active in the game.
        /// </summary>
        public bool IsActive { get; set; }

        private Vector2 _position;

        /// <summary>
        /// Gets or sets the current position of the entity. Validates the position against screen boundaries.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                var validation = new Rectangle((int)value.X, (int)value.Y, Area.Width, Area.Height);

                if (ScreenBoundaryChecker.IsInBound(validation))
                {
                    OnPositionChange(value); // Update position if within bounds
                }
                else
                {
                    OnPositionCanNotChange(); // Handle position out-of-bounds
                }
            }
        }

        /// <summary>
        /// Gets or sets the direction of movement for the entity as a normalized vector.
        /// </summary>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// Gets or sets the rectangular area that represents the entity's bounds.
        /// </summary>
        public Rectangle Area { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the entity.
        /// </summary>
        public float Velocity { get; set; }

        /// <summary>
        /// Gets or sets the visual representation of the entity.
        /// </summary>
        public PictureBox PictureBox { get; set; }

        /// <summary>
        /// The manager responsible for controlling and managing game entities.
        /// </summary>
        protected readonly IManagerGameEntities _entintyManager;

        /// <summary>
        /// Updates the entity's state, such as position and movement.
        /// </summary>
        public virtual void Update()
        {
            Move();
        }

        /// <summary>
        /// Moves the entity in the direction and velocity specified.
        /// </summary>
        private void Move()
        {
            int newX = (int)(Position.X + (Direction.X * Velocity));
            int newY = (int)(Position.Y + (Direction.Y * Velocity));

            Position = new Vector2(newX, newY); // Updates position and triggers bounds checking
            UpdatePictureBoxLocation(newX, newY);
        }

        /// <summary>
        /// Updates the location of the PictureBox to match the entity's position.
        /// </summary>
        /// <param name="x">The new X-coordinate for the PictureBox.</param>
        /// <param name="y">The new Y-coordinate for the PictureBox.</param>
        private void UpdatePictureBoxLocation(int x, int y)
        {
            if (PictureBox.InvokeRequired)
            {
                PictureBox.Invoke(new Action(() => PictureBox.Location = new Point(x, y)));
            }
            else
            {
                PictureBox.Location = new Point(x, y);
            }
        }

        /// <summary>
        /// Deactivates the entity and releases it back to the entity manager.
        /// </summary>
        public virtual void Disappear()
        {
            _entintyManager.ReleaseEntity(this);
            OnDisappear(); // Perform additional cleanup if necessary
        }

        /// <summary>
        /// Resets the entity's position and updates its visual representation.
        /// </summary>
        /// <param name="position">The new position to reset to.</param>
        /// <returns>A task representing the asynchronous reset operation.</returns>
        public async Task Reset(Vector2 position)
        {
            Position = position;

            int x = (int)position.X;
            int y = (int)position.Y;

            UpdatePictureBoxLocation(x, y);
            OnReset();
        }

        /// <summary>
        /// Handles any additional logic required during a reset.
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// Handles the logic when the entity's position is successfully updated.
        /// </summary>
        /// <param name="newValue">The new position of the entity.</param>
        protected virtual void OnPositionChange(Vector2 newValue)
        {
            _position = newValue;
            Area = new Rectangle((int)newValue.X, (int)newValue.Y, Area.Width, Area.Height);
            OnAreaChanged?.Invoke(this, EventArgs.Empty); // Trigger the area changed event
            UpdatePictureBoxLocation((int)_position.X, (int)_position.Y);
            HasChanged = true; // Mark the entity as updated
        }

        /// <summary>
        /// Handles the logic when the entity's position cannot be updated (e.g., out of bounds).
        /// </summary>
        protected virtual void OnPositionCanNotChange() { }

        /// <summary>
        /// Handles any additional logic when the entity disappears.
        /// </summary>
        protected virtual void OnDisappear() { }
    }
}


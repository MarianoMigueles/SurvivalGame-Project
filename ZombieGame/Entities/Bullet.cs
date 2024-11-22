using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.EntityManager;

namespace GunExtreme.Entities
{
    internal class Bullet : GameObject
    {
        private int _Damage { get; set; }

        private int _PositionX;  
        public override int PositionX
        {
            get
            {
                return _PositionX;
            }
            set
            {
                int screenWidth = GetFormWidth();
                if (value > (screenWidth - this.PictureBox.Width))
                {
                    this.HasChanged = false;
                    Disappear();
                }
                else if (value < 0)
                {
                    this.HasChanged = false;
                    Disappear();
                }
                else
                {
                    _PositionX = value;
                }
                    
            }
        }

        private int _PositionY;
        public override int PositionY
        {
            get
            {
                return _PositionY;
            }
            set
            {
                int screenHeight = GetFormHeight();
                if (value > (screenHeight + this.PictureBox.Height))
                {
                    this.HasChanged = false;
                    Disappear();
                }
                else if (value < 0)
                {
                    this.HasChanged = false;
                    Disappear();
                }
                else
                {
                    _PositionY = value;
                }
            }
        }

        public Bullet(int damage, int id, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
                    : base(velocity, pictureBox, entintyGenerator)
        {
            _Damage = damage;
        }

        public void Viajar(Point start ,Point destination)
        {
            this._PositionX = start.X;
            this._PositionY = start.Y;

            CalculateDirection(destination);

            HasChanged = true;
        }

        private void CalculateDirection(Point point)
        {
            float deltaX = point.X - this._PositionX;
            float deltaY = point.Y - this._PositionY;
            float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (magnitude > 0)
            {
                DirectionX = deltaX / magnitude;
                DirectionY = deltaY / magnitude;
            }
            else
            {
                DirectionX = 0;
                DirectionY = 0;
            }
        }

        protected override void OnReset()
        {
            this.DirectionX = 0;
            this.DirectionY = 0;
        }

    }
}

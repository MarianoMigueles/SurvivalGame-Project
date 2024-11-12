using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieGame.GameController.EntityManager;

namespace GunExtreme.Entities
{
    internal class Zombie : GameObject
    {
        private string Type { get;  set; }
        private int Damage { get; set; }

        private int _Health;
        public int Health
        {
            get
            {
                return _Health;
            }
            set
            {
                _Health = value;
                if (_Health <= 0)
                {
                    this.Disappear();
                }
            }
        }

        private int _PositionX;
        public override int PositionX
        {
            get
            {
                return _PositionX;
            }
            set
            {
                int screenWidth = this.GetFormWidth();
                if (value < 0)
                {
                    _PositionX = 0; 
                }
                else if (value > screenWidth)
                {
                    _PositionX = screenWidth; 
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
                int screenHeight = this.GetFormHeight();
                if (value < 0)
                {
                    _PositionY = 0;
                }
                else if (value > screenHeight)
                {
                    _PositionY = screenHeight;
                }
                else
                {
                    _PositionY = value;
                }

            }
        }

        public Zombie(string type, int damage,int health, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
                    : base(velocity, pictureBox, entintyGenerator)
        {
            Type = type;
            Health = health;
            Damage = damage;
        }

        public void Atack(Player player)
        {
            player.Health -= this.Damage;
        }

        public void TakeDamage()
        {
            throw new NotImplementedException();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities;
using ZombieGame.GameController.EntityManager;
using ZombieGame.Pools;

namespace GunExtreme.Entities
{
    internal class Player : GameObject
    {
        public string Name { get; set; }
        private int _Health;
        private Weapon _Weapon;
        private bool _IsRecharging = false;

        public int Health {
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
                    this.HasChanged = true;
                }
            }
        }

        private int _PositionX;
        public override int PositionX {
            get
            {
                return _PositionX;
            }
            set
            {
                int screenWidth = this.GetFormWidth();
                int halfPlayerArea = this.PictureBox.Width / 2;

                if (value > (screenWidth - halfPlayerArea))
                {
                    _PositionX = 0 - halfPlayerArea;
                }
                else if (value < (0 - halfPlayerArea))
                {
                    _PositionX = screenWidth - halfPlayerArea;
                }
                else
                {
                    _PositionX = value;
                }
                this.HasChanged = true;
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
                int halfPlayerArea = this.PictureBox.Height / 2;

                if (value > (screenHeight - halfPlayerArea))
                {
                    _PositionY = 0 - halfPlayerArea;
                }
                else if (value < (0 - halfPlayerArea))
                {
                    _PositionY = screenHeight - this.PictureBox.Height;
                }
                else
                {
                    _PositionY = value;
                }
                this.HasChanged = true;
            }
        }

        public Player(string name, int health, int id, int positionX, int positionY, int velocity, PictureBox pictureBox, IManagerGameEntities entintyGenerator)
                : base(velocity, pictureBox, entintyGenerator)
        {
            Name = name;
            Health = health;

            _Weapon = new Weapon(
                "pistol","hand",/*15*/1,500
            );

            _ = Recharge();
        }

        public async void Shoot()
        {           
            if (!this._IsRecharging)
            {
                if (!_Weapon.CanShoot)
                {
                    return;
                }

                if (_Weapon.Bullets.Count > 0)
                {
                    Point mousePosition = _entintyManager.GetMousePosition();

                    _Weapon.CanShoot = false;

                    Point playerLocatetion = new Point()
                    {
                        X = this.PositionX,
                        Y = this.PositionY
                    };

                    Bullet bullet = _Weapon.Bullets[0];
                    _Weapon.Bullets.RemoveAt(0);
                    _entintyManager.AddEntity(bullet);
                    bullet.Viajar(playerLocatetion , mousePosition);

                    await Task.Delay(_Weapon.FireRateDelay);

                    _Weapon.CanShoot = true;
                }
                else
                {
                    await Recharge();
                }
            }
        }

        public void TakeDamage(int damage)
        {
            this._Health -= damage;
        }

        private async Task Recharge()
        {
            this._IsRecharging = true;
            for (int i = 0; i < _Weapon.AmountBullets; i++)
            {
                Bullet newBullet = _entintyManager.GenerateNewBullet();
                _Weapon.Bullets.Add(newBullet);
            }
            this._IsRecharging = false;
        }


    }
}

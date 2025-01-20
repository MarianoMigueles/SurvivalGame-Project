using GunExtreme.Entities;
using ZombieGame;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Weapons;
using ZombieGame.GameController.EntityManager;

namespace ZombieGame.Entities.Players
{
    public interface IPlayer : ICollidable, IReleaseble
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Murders { get; set; }
        public PictureBox PictureBox { get; set; }

        public Rectangle ViewArea { get; set; }

        public void Shoot();
        public void TakeDamage(int damage);
        public Task RechargeAsync();
    }
}

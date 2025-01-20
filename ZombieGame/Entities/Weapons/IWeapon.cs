using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Ammunition;

namespace ZombieGame.Entities.Weapons
{
    public interface IWeapon
    {
        public string Type { get; set; }
        public string Model { get; set; }
        public int AmountBullets { get; set; }
        public int FireRateDelay { get; set; }
        public bool CanShoot { get; set; }
        public List<Bullet> Bullets { get; set; }
    }
}

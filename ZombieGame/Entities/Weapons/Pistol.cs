using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Ammunition;
using ZombieGame.GameController.EntityManager;

namespace ZombieGame.Entities.Weapons
{
    public class Pistol(string type, string model, int amountBullets, int fireRateDelay) : IWeapon
    {
        public string Type { get; set; } = type;
        public string Model { get; set; } = model;
        public int AmountBullets { get; set; } = amountBullets;
        public int FireRateDelay { get; set; } = fireRateDelay;
        public bool CanShoot { get; set; } = true;
        public List<Bullet> Bullets { get; set; } = [];
    }
}

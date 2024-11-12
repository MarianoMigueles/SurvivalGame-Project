using GunExtreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.EntityManager;

namespace ZombieGame.Entities
{
    internal class Weapon(string type, string model, int amountBullets, int fireRateDelay)
    {
        public string Type { get; set; } = type;
        public string Model { get; set; } = model;
        public int AmountBullets { get; set; } = amountBullets;
        public int FireRateDelay { get; set; } = fireRateDelay;
        public bool CanShoot { get; set; } = true;
        public List<Bullet> Bullets { get; set; } = [];
    }
}

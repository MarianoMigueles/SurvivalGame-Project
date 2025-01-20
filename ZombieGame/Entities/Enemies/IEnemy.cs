using ZombieGame;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Players;
using ZombieGame.GameController.EntityManager;
using Utilities.DirectionalUtilities;

namespace ZombieGame.Entities.Enemies
{
    public interface IEnemy : ICollidable, IDirectional, IReleaseble
    {
        public string Type { get; set; }
        public int Damage { get; set; }
        public int Health { get; set; }
        public bool IsAttacking { get; set; }
        public PictureBox PictureBox { get; set; }

        public void Atack(IPlayer player);
        public void TakeDamage(int damage);
        public void Chase();
        public void CalculateDirection(Vector2 destination);
    }
}

using ZombieGame;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.EntityManager;
using Utilities.DirectionalUtilities;

namespace ZombieGame.Entities.Ammunition
{
    public interface IAmmunition : ICollidable, IDirectional, IReleaseble
    {
        public int Damage { get; set; }
        public PictureBox PictureBox { get; set; }
        public void Travel(Point start, Point target);
        public void CalculateDirection(Vector2 destination);
    }
}

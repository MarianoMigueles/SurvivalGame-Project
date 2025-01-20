using ZombieGame.Entities.Players;
using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGameTests.MockObjects.Entities;

namespace ZombieGameTests.MockObjects.ManagerEnitites
{
    public class MockManagerEntities : IMockManagerEntities
    {
        private IQuadtree<ICollidable> _quadtree;
        private IPlayer _player;
        public MockManagerEntities(IPlayer player, IQuadtree<ICollidable> quadtree)
        {
            _player = player;
            _quadtree = quadtree;
        }

        public Point GetPlayerPoint()
        {
            Point point = new Point(_player.Area.X, _player.Area.Y);
            return point;
        }

        public void ReleaseEntity(ICollidable obj)
        {
            _quadtree.Remove(obj);
        }
    }
}

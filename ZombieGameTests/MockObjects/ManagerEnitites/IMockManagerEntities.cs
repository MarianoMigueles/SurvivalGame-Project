using GunExtreme.Entities;
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
    public interface IMockManagerEntities
    {
        public Point GetPlayerPoint();
        public void ReleaseEntity(ICollidable obj);
    }
}

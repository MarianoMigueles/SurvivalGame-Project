using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGameTests.MockObjects.SpatialStructures
{
    public class MockCollidable : ICollidable
    {
        public Rectangle Area { get; set; }
        public bool IsReinserting { get; set; }
        public bool IsActive { get; set; }

        public MockCollidable(Rectangle area)
        {
            Area = area;
        }

        public event EventHandler OnAreaChanged;
        public event Action OnNotify;

        public void ChangeArea(Rectangle newArea)
        {
            Area = newArea;
            OnAreaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

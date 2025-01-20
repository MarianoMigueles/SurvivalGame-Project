using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieGame.Entities.Ammunition;
using ZombieGame.GameController.EntityManager;
using SpatialStructures;

namespace ZombieGameTests.MockObjects.Entities
{
    internal class MockBullet : IAmmunition
    {
        public MockBullet() { }

        public MockBullet(Rectangle area)
        {
            Area = area;
        }

        public MockBullet(int damage, Rectangle area, PictureBox picture = null)
        {
            Damage = damage;
            Area = area;

            if (picture != null) PictureBox = picture;
        }

        public Rectangle Area { get; set; } = new Rectangle();
        public int Damage { get; set; } = 1;
        public PictureBox PictureBox { get; set; } = new PictureBox();
        public Vector2 Direction { get; set; }
        public bool IsReinserting { get; set; }

        public event EventHandler OnAreaChanged;

        public bool IsActive { get; set; }
        public void CalculateDirection(Vector2 destination)
        {
            Console.WriteLine("The bullet direction was calculated");
        }

        public void Disappear()
        {
            Console.WriteLine("The bullet disapear");
        }

        public void Travel(Point start, Point target)
        {
            Console.WriteLine("The bullet is moving");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieGame.Entities.Players;
using ZombieGame.GameController;
using ZombieGame.GameController.EntityManager;

namespace ZombieGameTests.MockObjects.Entities
{
    internal class MockPlayer : IPlayer
    {
        public MockPlayer() { }

        public MockPlayer(Rectangle area)
        {
            Area = area;
        }

        public MockPlayer(string name, int health, Rectangle area, PictureBox picture = null)
        {
            Name = name;
            Health = health;
            Area = area;

            if (picture != null) PictureBox = picture;
        }

        public string Name { get; set; } = "Player";
        public int Health { get; set; } = 10;
        public PictureBox PictureBox { get; set; } = new PictureBox();
        public Rectangle Area { get; set; } = new Rectangle(0, 0, 10, 10);
        public bool IsReinserting { get; set; }
        public bool IsActive { get; set; }

        public event EventHandler OnAreaChanged;
        public async Task RechargeAsync()
        {
            Console.WriteLine("The player is recharging");
        }

        public void Shoot()
        {
            Console.WriteLine("The player is shooting");
        }

        public void TakeDamage(int damage)
        {
            Console.WriteLine("The player is taking damage");
        }
    }
}

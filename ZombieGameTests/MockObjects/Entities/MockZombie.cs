using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieGame.Entities.Enemies;
using ZombieGame.Entities.Players;
using ZombieGame.GameController;
using ZombieGame.GameController.EntityManager;
using Utilities.DirectionalUtilities;

namespace ZombieGameTests.MockObjects.Entities
{
    internal class MockZombie : IEnemy
    {
        public MockZombie() { }

        public MockZombie(Rectangle area)
        {
            Area = area;
        }

        public MockZombie(string type, int health, int damage, Rectangle area, PictureBox picture = null)
        {
            Type = type;
            Health = health;
            Area = area;

            if (picture != null) PictureBox = picture;
        }

        public Rectangle Area { get; set; } = new Rectangle(0, 0, 10, 10);
        public string Type { get; set; } = "Defauld";
        public int Damage { get; set; } = 1;
        public int Health { get; set; } = 10;
        public PictureBox PictureBox { get; set; } = new PictureBox();
        public Vector2 Direction { get; set; }
        public bool IsReinserting { get; set; }

        public event EventHandler OnAreaChanged;

        public bool IsActive { get; set; }
        public void Atack(IPlayer player)
        {
            player.Health -= Damage;
            player.PictureBox.BackColor = Color.Red;
        }

        public void CalculateDirection(Vector2 destination)
        {
            Direction = DirectionHelper.CalculateDirection(new Vector2(Area.X, Area.Y), destination);
        }

        public void Chase()
        {
            Console.WriteLine("The zombie is chasing player");
        }

        public void Disappear()
        {
            Console.WriteLine("zombie was killed");
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}

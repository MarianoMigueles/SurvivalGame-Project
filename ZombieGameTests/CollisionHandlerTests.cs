using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieGame.Entities.Enemies;
using ZombieGame.Entities.Players;
using ZombieGame.GameController;
using ZombieGameTests.MockObjects.ManagerEnitites;
using ZombieGameTests.MockObjects.Entities;

namespace ZombieGameTests
{
    [TestFixture]
    public class CollisionHandlerTests
    {
        private Quadtree<ICollidable> _mockQuadtree;
        private MockPlayer _mockPlayer;
        private MockZombie _mockZombie;

        [SetUp]
        public void Setup()
        {
            _mockQuadtree = new(new Rectangle(0, 0, 500, 500));

            _mockPlayer = new MockPlayer();
            _mockZombie = new MockZombie();

            CollisionHandler.InitializeCollisionGame(_mockPlayer, _mockQuadtree);
        }

        [Test]
        public void CheckCollisions_PlayerCollidesWithZombieInTheSameLocate_ZombieAttacksPlayer()
        {
            _mockQuadtree.Insert(_mockPlayer, _mockPlayer.Area);
            _mockQuadtree.Insert(_mockZombie, _mockZombie.Area);

            CollisionHandler.CheckCollisions();

            Assert.That(_mockPlayer.Health, Is.EqualTo(9));
        }

        [Test]
        public void CheckCollisions_PlayerBoundCollidesWithZombieBound_ZombieAttacksPlayer()
        {
            _mockPlayer.Area = new Rectangle(0, 0, 20, 20);
            _mockZombie.Area = new Rectangle(15, 15, 20, 20);
            _mockQuadtree.Insert(_mockPlayer, _mockPlayer.Area);
            _mockQuadtree.Insert(_mockZombie, _mockZombie.Area);

            CollisionHandler.CheckCollisions();

            Assert.That(_mockPlayer.Health, Is.EqualTo(9));
        }

        [Test]
        public void CheckCollisions_PlayerNotCollidesWithZombieBound_ZombieNotAttacksPlayer()
        {
            _mockPlayer.Area = new Rectangle(0, 0, 20, 20);
            _mockZombie.Area = new Rectangle(20, 20, 20, 20);
            _mockQuadtree.Insert(_mockPlayer, _mockPlayer.Area);
            _mockQuadtree.Insert(_mockZombie, _mockZombie.Area);

            CollisionHandler.CheckCollisions();

            Assert.That(_mockPlayer.Health, Is.EqualTo(10));
        }
    }
}

using Moq;
using SpatialStructures;
using System.Drawing;
using System.Reflection;
using ZombieGameTests.MockObjects.SpatialStructures;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ZombieGameTests
{
    public class SpatialStructuresTests
    {
        private Mock<MockQuadtree> _quadTree;

        [SetUp]
        public void Setup()
        {
            _quadTree = new Mock<MockQuadtree>(new Rectangle(10, 10, 500, 500))
            {
                CallBase = true
            };
        }

        [Test]
        public void Insert_ObjectIntoQuadtree_ShouldInsertSuccessfully()
        {
            var obj = new MockCollidable(new Rectangle(10, 10, 10, 10));

            _quadTree.Object.Insert(obj, obj.Area);

            Assert.That(_quadTree.Object.Count, Is.EqualTo(1));
        }

        [Test]
        public void Insert_ObjectsExceedMaxLimit_ShouldSubdivideAndRedistribute()
        {
            _quadTree = new Mock<MockQuadtree>(new Rectangle(0, 0, 100, 100), 2, 5) { CallBase = true };

            var obj1 = new MockCollidable(new Rectangle(0, 0, 10, 10));
            var obj2 = new MockCollidable(new Rectangle(100, 0, 10, 10));
            var obj3 = new MockCollidable(new Rectangle(0, 80, 10, 10));

            _quadTree.Object.Insert(obj1, obj1.Area);
            _quadTree.Object.Insert(obj2, obj2.Area);
            _quadTree.Object.Insert(obj3, obj3.Area);

            Assert.That(_quadTree.Object.Count, Is.EqualTo(3));
            Assert.That(_quadTree.Object.GetAllBounds(), Has.Count.EqualTo(5));
        }

        [Test]
        public void ReinsertObjectForEvent_ObjectShouldBeReinserted()
        {
            var obj = new MockCollidable(new Rectangle(0, 0, 10, 10));
            _quadTree.Object.Insert(obj, obj.Area);
            var newBounds = new Rectangle(50, 50, 10, 10);
            _quadTree.Setup(q => q.Insert(It.IsAny<MockCollidable>(), It.IsAny<Rectangle>())).Verifiable("Retorno");   

            obj.ChangeArea(newBounds);

            Assert.That(_quadTree.Object.Count, Is.EqualTo(1));
            Assert.That(_quadTree.Object.Retrieve(newBounds).FirstOrDefault()?.Area, Is.EqualTo(newBounds));
        }

        //[Test]
        //public void ReinserObjectForEvent_EventIsSuscribed_ObjectShouldChangeEventSuscribed()
        //{
        //    var obj = new MockCollidable(new Rectangle(0, 0, 10, 10));
        //    _quadTree.Object.Subdivide();
        //    _quadTree.Object.Insert(obj, obj.Area);
        //    int subscriberCount = 0;
        //    obj.OnAreaChanged += (sender, e) => { subscriberCount++; };

        //    obj.ChangeArea(new(450, 0, 10, 10));

        //    Assert.That(subscriberCount, Is.GreaterThan(0));
        //}

        [Test]
        public void ReinsertObjectForEvent_ObjectShouldBeReinsertedTwice()
        {
            var obj = new MockCollidable(new Rectangle(0, 0, 10, 10));
            _quadTree.Object.Subdivide();
            _quadTree.Object.Insert(obj, obj.Area);
            var firtArea = new Rectangle(450, 0, 10,10);
            var secondArea = new Rectangle(10, 0, 10, 10);

            obj.ChangeArea(firtArea);
            obj.ChangeArea(secondArea);

            //Assert.That(_quadTree.Object.);
        }

        [Test]
        public void ChangeObjectPosition_ObjectMoveOutQuadtreeArea_ObjectShouldBeRemoved()
        {
            var obj = new MockCollidable(new Rectangle(0, 0, 10, 10));
            _quadTree.Object.Insert(obj, obj.Area);
            var newArea = new Rectangle(400, 0, 10, 10);

            obj.ChangeArea(newArea);
            _quadTree.Object.Remove(obj);

            Assert.That(_quadTree.Object.Count, Is.EqualTo(0));
        }
    }
}
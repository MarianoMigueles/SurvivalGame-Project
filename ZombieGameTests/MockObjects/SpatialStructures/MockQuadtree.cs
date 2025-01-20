using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGameTests.MockObjects.SpatialStructures
{
    public class MockQuadtree : IQuadtree<MockCollidable>
    {
        private readonly Quadtree<MockCollidable> _quadtree;
        public MockQuadtree(Rectangle bound) 
        {
            _quadtree = new Quadtree<MockCollidable>(bound, id: 0, level: 0, maxObjs: 4, maxLvls: 5, parent: null);
        }

        public MockQuadtree(Rectangle bound, int maxObjs, int maxLvls)
        {
            _quadtree = new Quadtree<MockCollidable>(bound, id: 0, level: 0, maxObjs, maxLvls, parent: null);
        }

        public MockQuadtree(Rectangle bound, int id, int level = 0, int maxObjs = 4, int maxLvls = 5, Quadtree<MockCollidable> parent = null)
        {
            _quadtree = new Quadtree<MockCollidable>(bound, id, level, maxObjs, maxLvls, parent);
        }

        public virtual int Count => _quadtree.Count;
        public virtual Quadtree<MockCollidable>[] GetSubQuadtrees()
        {
            var quadtreeSubdivide = typeof(Quadtree<MockCollidable>)
                .GetMethod("_subQuadrants", BindingFlags.NonPublic | BindingFlags.Instance);

            var objs = quadtreeSubdivide.Invoke(_quadtree, new object[] { });
            return new Quadtree<MockCollidable>[4];
        }

        public virtual void Clear() => _quadtree.Clear();

        public virtual List<Rectangle> GetAllBounds() => _quadtree.GetAllBounds();

        public virtual void Insert(MockCollidable obj, Rectangle objBounds) => _quadtree.Insert(obj, objBounds);

        public virtual void ReinsertObject(MockCollidable obj) => _quadtree.ReinsertObject(obj);

        public virtual bool Remove(MockCollidable obj) => _quadtree.Remove(obj);

        public virtual void Resize(int newWidth, int newHeight) => _quadtree.Resize(newWidth, newHeight);

        public virtual void Subdivide()
        {
            _quadtree.Subdivide();
            //var quadtreeSubdivide = typeof(Quadtree<MockCollidable>)
            //                .GetMethod("Subdivide", BindingFlags.NonPublic | BindingFlags.Instance);

            //quadtreeSubdivide.Invoke(_quadtree, new object[] { });
        }

        public virtual List<MockCollidable> Retrieve(Rectangle area) => _quadtree.Retrieve(area);

        public virtual bool IsOnBoundary(Rectangle obj)
        {
            var quadtreeBounds = _quadtree.Size;

            if(quadtreeBounds.Contains(obj))
            {
                return true;
            }

            return false;
        }
    }
}

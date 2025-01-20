using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialStructures
{
    public interface IQuadtree<T> 
    {
        public void Insert(T obj, Rectangle objBounds);
        public bool Remove(T obj);
        public void ReinsertObject(T obj);
        public List<T> Retrieve(Rectangle area);
        public void Resize(int newWidth, int newHeight);
        public List<Rectangle> GetAllBounds();
        public void Clear();
        public int Count { get; }
    }
}

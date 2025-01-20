using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpatialStructures
{
    /// <summary>
    /// Interface that enforces objects to have bounds, required for use with QuadTree.
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Gets the bounds of the object as a Rectangle.
        /// </summary>
        public Rectangle Area { get; set; }
        public bool IsReinserting { get; set; }
        public bool IsActive { get; set; }
        //public int QuadtreeId { get; set; }
        /// <summary>
        /// Event triggered when the area changes.
        /// </summary>
        event EventHandler OnAreaChanged;
    }
}

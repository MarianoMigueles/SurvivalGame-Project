using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DirectionalUtilities
{
    public interface IDirectional
    {
        public Vector2 Direction { get; set; }
        public void CalculateDirection(Vector2 destination);
    }
}

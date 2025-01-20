using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialStructures.Notifier
{
    [Flags]
    public enum ENotificationType
    {
        None = 0,
        OnInsert = 1 << 0,
        OnRemove = 1 << 1,
        OnSubscribe = 1 << 2,
        OnUnsuscribe = 1 << 3,
        OnObjectPositionChange = 1 << 4,
        OnObjectPositionCanNotChange = 1 << 5,
        OnObjectReinserted = 1 << 6,
        OnResize = 1 << 7,
        OnSubdivide = 1 << 8,
        OnClear = 1 << 9
    }
}

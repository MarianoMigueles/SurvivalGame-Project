using ZombieGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame
{
    /// <summary>
    /// A generic object pool for managing reusable objects of type <typeparamref name="T"/> that implement <see cref="IIdentificable"/>.
    /// The pool holds both available and in-use objects, and assigns unique IDs to each object.
    /// </summary>
    public class ObjectPool<T> where T : IIdentificable
    {
        private readonly List<T> AvailableObjects;
        private readonly Dictionary<int, T> InUseObjects;
        private readonly IdManager _idManager;

        /// <summary>
        /// Gets a combined list of all objects, both available and in-use.
        /// </summary>
        public List<T> Objects
        {
            get
            {
                List<T> objects = new List<T>();
                objects.AddRange(AvailableObjects);
                objects.AddRange(InUseObjects.Values);

                return objects;
            }
        }

        /// <summary>
        /// Gets a list of all currently in-use objects.
        /// </summary>
        public List<T> ObjectsActive
        {
            get
            {
                return [.. InUseObjects.Values];
            }
        }

        /// <summary>
        /// Gets a list of all currently available objects.
        /// </summary>
        public List<T> ObjectsInactive
        {
            get
            {
                return [.. AvailableObjects];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        public ObjectPool()
        {
            AvailableObjects = [];
            InUseObjects = [];
            _idManager = new IdManager();
        }

        /// <summary>
        /// Retrieves an object from the pool. If no objects are available, a new one is created using the provided factory method.
        /// </summary>
        /// <param name="factory">A function that creates a new object if none are available in the pool.</param>
        /// <returns>The object retrieved from the pool.</returns>
        public T GetObject(Func<T> factory)
        {
            T obj;

            if (AvailableObjects.Count > 0)
            {
                obj = AvailableObjects[0];
                AvailableObjects.RemoveAt(0);
                obj.IsFromPool = true;
            }
            else
            {
                obj = factory();
            }

            int id = _idManager.GetId();
            ((IIdentificable)obj).Id = id;

            InUseObjects.Add(obj.Id.Value, obj);
            return obj;
        }

        /// <summary>
        /// Releases an object back into the pool. It must be in-use, otherwise an exception is thrown.
        /// </summary>
        /// <param name="obj">The object to release back into the pool.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ReleaseObject(T obj)
        {
            if (obj.Id == null)
            {
                throw new Exception("The item have not Id.");
            }

            if (!InUseObjects.Remove(obj.Id.Value))
            {
                throw new Exception("The item is not in use or has already been released.");
            }

            _idManager.ReleaseId(((IIdentificable)obj).Id.Value);
            obj.Id = null;

            AvailableObjects.Add(obj);
        }
    }
}

using GunExtreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Pools
{
    internal class ObjectPool
    {
        public readonly List<GameObject> AvailableObjects;
        public readonly HashSet<GameObject> InUseObjects;
        public ObjectPool()
        {
            AvailableObjects = [];//==new List<GameObject>();
            InUseObjects = [];//==new HashSet<GameObject>();
        }

        //public ObjectPool(int cantOfObjects, Func<int, T> objectGenerator)
        //{
        //    _availableObjects = [];//==new List<GameObject>();
        //    _inUseObjects = [];//==new HashSet<GameObject>();

        //    for (int i = 0; i < cantOfObjects; i++)
        //    {
        //        _availableObjects.Add(objectGenerator(i));
        //    }
        //}

        public T GetObject<T>(Func<PictureBox, T> factory, Action<T> resetAction) where T : GameObject
        {
            T obj;

            if(AvailableObjects.Count > 0)
            {
                obj = (T)AvailableObjects[0];
                AvailableObjects.RemoveAt(0);
            } 
            else
            {
                obj = factory(new PictureBox());
            }

            InUseObjects.Add(obj);
            resetAction(obj);
            return obj;
        }

        public void ReleaseObject(GameObject obj)
        {
            if (!InUseObjects.Remove(obj))
            {
                throw new Exception("El objeto no está en uso o ya ha sido liberado.");
            }
            
            AvailableObjects.Add(obj);
            obj.Reset(0, 0);
        }
    }
}

using PoolClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.GameController.Managers;

namespace ZombieGame.Pools
{
    public class ObjectPool<T> where T : IIdentificable
    {
        public readonly List<T> AvailableObjects;
        public readonly Dictionary<int, T> InUseObjects;
        public ObjectPool()
        {
            AvailableObjects = [];
            InUseObjects = [];
        }

        public T GetObject(Func<T> factory)
        {
            T obj;

            if(AvailableObjects.Count > 0)
            {
                obj = AvailableObjects[0];
                AvailableObjects.RemoveAt(0);
            } 
            else
            {
                obj = factory();
            }

            int id = IdManager.GetId();
            ((IIdentificable)obj).Id = id;

            InUseObjects.Add(obj.Id ,obj);
            return obj;
        }

        public void ReleaseObject(T obj)
        {
            if (!InUseObjects.Remove(obj.Id))
            {
                throw new Exception("The item is not in use or has already been released.");
            }

            IdManager.ReleaseId(((IIdentificable)obj).Id);

            AvailableObjects.Add(obj);
        }
    }
}

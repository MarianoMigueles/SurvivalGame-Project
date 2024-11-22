using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.GameController.Managers
{
    internal static class IdManager
    {
        private static Queue<int> _availableIds = [];
        private static HashSet<int> _inUseIds = [];
        private static int _counter = 0;

        public static int GetId()
        {
            int id;

            if (_availableIds.Count > 0)
            {
                id = _availableIds.Dequeue();
            }
            else
            {
                id = _counter;
                _counter++;
            }

            _inUseIds.Add(id);
            return id;
        }

        public static void ReleaseId(int id)
        {
            if (_inUseIds.Remove(id))
            {
                _availableIds.Enqueue(id);
            } 
            else
            {
                throw new Exception($"The index {id} is not in use or has already been released.");
            }
        }
    }
}

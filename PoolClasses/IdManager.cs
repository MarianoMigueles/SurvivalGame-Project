using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Manages a pool of unique IDs. It generates new IDs or reuses previously released ones.
/// </summary>
namespace ZombieGame
{
    internal class IdManager
    {
        private readonly Queue<int> _availableIds;
        private readonly HashSet<int> _inUseIds;
        private int _counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdManager"/> class.
        /// </summary>
        public IdManager()
        {
            _availableIds = new Queue<int>();
            _inUseIds = new HashSet<int>();
            _counter = 0;
        }

        /// <summary>
        /// Gets a unique ID, either by reusing a released ID or by generating a new one.
        /// </summary>
        /// <returns>A unique ID.</returns>
        public int GetId()
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

        /// <summary>
        /// Releases an ID that is no longer in use, allowing it to be reused.
        /// </summary>
        /// <param name="id">The ID to release.</param>
        /// <exception cref="Exception">Thrown when attempting to release an ID that is not in use or has already been released.</exception>
        public void ReleaseId(int id)
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


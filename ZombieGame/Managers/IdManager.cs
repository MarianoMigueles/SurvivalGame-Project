using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Managers
{
    internal static class IdManager
    {
        private static Queue<int> _availableIds;
        private static HashSet<int> _inUseIds;

        static IdManager()
        {
            
        }

        public static int GetId()
        {
            throw new NotImplementedException();
        }

        public static void ReleaseId(int Id)
        {
            throw new NotImplementedException();
        }
    }
}

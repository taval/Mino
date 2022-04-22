using System;
using System.Collections.Generic;
using System.Linq;



namespace ViewModelExtended
{
    public static class Utility
    {
        public static List<T> CloneList<T> (this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static long UnixDateTime ()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }
    }
}

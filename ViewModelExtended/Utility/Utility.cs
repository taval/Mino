using System;
using System.Collections.Generic;
using System.Linq;



namespace ViewModelExtended
{
    public static class Utility
    {
		/// <summary>
		/// clone a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listToClone"></param>
		/// <returns></returns>
		public static List<T> CloneList<T> (this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// return date/time in Unix timestamp format
        /// </summary>
        /// <returns></returns>
        public static long UnixDateTime ()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }

        public static DateTime UnixToDateTime (long unixTime)
        {
            var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
            return localDateTimeOffset;
        }
    }
}

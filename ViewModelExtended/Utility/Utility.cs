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

        // TODO: this is to trigger a NotifyPropertyChanged event for each obj in order for the index display to be updated. Displaying a massive amount of indices could be a drag. A mitigation would be to use a range (all adjacent nodes, for instance). In mean time, remove calls to this method and displaying of indices if it becomes a problem
        public static void RefreshListView (IEnumerable<IListItem> items)
        {
            foreach (IListItem obj in items) {
                int? _ = obj.PreviousId;
                _ = obj.NextId;
            }
        }
    }
}

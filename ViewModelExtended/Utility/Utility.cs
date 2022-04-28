using System;
using System.Collections.Generic;
using System.Linq;



namespace ViewModelExtended
{
    public static class Utility
    {
		/// <summary>
		/// create the relationship between Notes in a linked list
		/// assumes note begin/end nodes will correctly contain null upon immediate construction
		/// </summary>
		/// <param name="items"></param>
		public static void LinkAll (IEnumerable<IListItem> items)
		{
			IListItem? prev = null;
			IListItem? current = null;
			IListItem? next = null;

			foreach (IListItem note in items) {
				// set first node
				if (prev == null) {
					prev = note;
					continue;
				}

				// set second node
				if (prev != null && current == null) {
					current = note;
					continue;
				}

				// connect subsequent nodes
				if (prev != null && current != null) {
					next = note;
					prev.Next = current;
					current.Previous = prev;
					current.Next = next;
					next.Previous = current;
					prev = current;
					current = next;
				}
			}
		}

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

        public static long UnixDateTime ()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }

        // TODO: this is to trigger a NotifyPropertyChanged event for each obj in order for the index display to be updated. Displaying a massive amount of indices could be very slow. A mitigation would be to use a range (all adjacent nodes, for instance). In mean time, remove calls to this method and displaying of indices if it becomes a problem
		/// <summary>
		/// refresh list display
		/// </summary>
		/// <param name="items"></param>
        public static void RefreshListView (IEnumerable<IListItem> items)
        {
            foreach (IListItem obj in items) {
                int? _ = obj.PreviousId;
                _ = obj.NextId;
            }
        }
    }
}

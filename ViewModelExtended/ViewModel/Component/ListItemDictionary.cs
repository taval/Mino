using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// a dictionary for storing unrelated IListItems
	/// the index is for sorting the objects in ascending order
	/// useful as a queue with lookup via key
	/// in contrast with the IListItem-aware IObservableList, this is intended as a dependent collection.
	/// Data should not be stored here, only referenced.
	/// </summary>
	internal class ListItemDictionary : IListItemDictionary
	{
		private Dictionary<IListItem, int> f_Items;

		private IdGenerator f_IdGenerator;

		public int Count {
			get {
				return f_Items.Count;
			}
		}

		public ListItemDictionary ()
		{
			f_IdGenerator = new IdGenerator();
			f_Items = new Dictionary<IListItem, int>(new ListItemEqualityComparer());
		}

		public ListItemDictionary (Dictionary<IListItem, int> dictionary)
		{
			f_IdGenerator = new IdGenerator();
			f_Items = dictionary;
		}

		private IEnumerable<KeyValuePair<IListItem, int>> GetSorted ()
		{
			return
				from item in f_Items
				orderby item.Value ascending
				select item;
		}

		public bool ContainsKey (IListItem key)
		{
			return f_Items.ContainsKey(key);
		}

		public void Add (IListItem input)
		{
			f_Items.Add(input, f_IdGenerator.Generate());
		}

		public void Remove (IListItem input)
		{
			f_Items.Remove(input);
		}

		public void Clear ()
		{
			f_Items.Clear();
		}

		public bool Any ()
		{
			return f_Items.Any();
		}

		public IEnumerator<KeyValuePair<IListItem, int>> GetEnumerator ()
		{
			IEnumerable<KeyValuePair<IListItem, int>> sortedChanges = GetSorted();

			return f_Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
	}
}

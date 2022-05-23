using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;



// TODO: marked for internal use only, similarly to GroupContents and the ChangeQueue variants.
//		 does not interface with any view or external code.

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// a dictionary for storing unrelated IListItems
	/// the index is for sorting the objects in ascending order
	/// useful as a queue with lookup via key
	/// in contrast with the IListItem-aware IObservableList, this is intended as a dependent collection.
	/// Data should not be stored here, only referenced.
	/// </summary>
	public class ListItemDictionary : IEnumerable<KeyValuePair<IListItem, int>>
	{
		//public Dictionary<IListItem, int> Items { get; private set; }
		private Dictionary<IListItem, int> f_Items;

		private IdGenerator f_IdGenerator;

		public int Count {
			get {
				return f_Items.Count;
			}
		}

		private IViewModelResource f_Resource;

		public ListItemDictionary (IViewModelResource resource)
		{
			f_IdGenerator = new IdGenerator();
			f_Resource = resource;
			f_Items = resource.ViewModelCreator.CreateDictionary();
		}

		public ListItemDictionary (IViewModelResource resource, Dictionary<IListItem, int> dictionary)
		{
			f_IdGenerator = new IdGenerator();
			f_Resource = resource;
			f_Items = dictionary;
		}

		private IEnumerable<KeyValuePair<IListItem, int>> Sort ()
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

		public IEnumerator<KeyValuePair<IListItem, int>> GetEnumerator ()
		{
			IEnumerable<KeyValuePair<IListItem, int>> sortedChanges = Sort();

			return f_Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
	}
}

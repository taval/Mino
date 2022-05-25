using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	internal interface IListItemDictionary : IEnumerable<KeyValuePair<IListItem, int>>
	{
		public int Count { get; }

		public bool ContainsKey (IListItem key);

		public void Add (IListItem input);

		public void Remove (IListItem input);

		public bool Any ();

		public void Clear ();
	}
}

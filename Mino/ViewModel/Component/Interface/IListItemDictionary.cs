using System.Collections.Generic;



namespace Mino.ViewModel
{
	internal interface IListItemDictionary : IEnumerable<KeyValuePair<IListItem, int>>
	{
		public IReadOnlyDictionary<IListItem, int> Items { get; }

		public int Count { get; }

		public bool ContainsKey (IListItem key);

		public void Add (IListItem input);

		public void Remove (IListItem input);

		public bool Any ();

		public void Clear ();
	}
}

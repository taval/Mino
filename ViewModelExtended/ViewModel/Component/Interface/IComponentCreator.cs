using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	internal interface IComponentCreator
	{
		#region List

		public IObservableList<T> CreateObservableList<T> () where T : IListItem;

		#endregion



		#region Dictionary

		public IListItemDictionary CreateListItemDictionary ();

		public IListItemDictionary CreateListItemDictionary (Dictionary<IListItem, int> dictionary);

		#endregion



		#region ChangeQueue

		public IChangeQueue<T> CreateChangeQueue<T> () where T : IListItem;

		public IChangeQueue<T> CreateChangeQueue<T> (Dictionary<IListItem, int> dictionary) where T : IListItem;

		#endregion
	}
}

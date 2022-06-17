using System;
using System.Collections.Generic;
using System.Text;



namespace Mino.ViewModel
{
	internal class ComponentCreator : IComponentCreator
	{
		#region Constructor

		public ComponentCreator ()
		{
			
		}

		#endregion



		#region List

		public IObservableList<T> CreateObservableList<T> () where T : IListItem
		{
			return new ObservableList<T>();
		}

		#endregion



		#region Dictionary

		public IListItemDictionary CreateListItemDictionary ()
		{
			return new ListItemDictionary();
		}

		public IListItemDictionary CreateListItemDictionary (Dictionary<IListItem, int> dictionary)
		{
			return new ListItemDictionary(dictionary);
		}

		#endregion



		#region ChangeQueue

		public IChangeQueue<T> CreateChangeQueue<T> () where T : IListItem
		{
			return new ChangeQueue<T>(() => CreateListItemDictionary());
		}

		public IChangeQueue<T> CreateChangeQueue<T> (Dictionary<IListItem, int> dictionary) where T : IListItem
		{
			return new ChangeQueue<T>((d) => CreateListItemDictionary(d), dictionary);
		}

		#endregion
	}
}

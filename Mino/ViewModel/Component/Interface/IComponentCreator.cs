using System;
using System.Collections.Generic;
using System.Text;

/** NOTE: When creating objects with method references, a reference to their source must be kept like anything else,
 *        as long as it is active and not ready for gc.
 *        To that end, store a reference to IComponentCreator in the viewmodel when using jt to build viewmodel components.
 */

namespace Mino.ViewModel
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

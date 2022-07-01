using System.Collections.Generic;
using Mino.Model;



namespace Mino.ViewModel
{
	public interface IDbListHelper
	{
		public void UpdateNodes (IDbContext dbContext, IListItem? item);

		public void UpdateAfterAdd (IDbContext dbContext, IListItem item);
		public void UpdateAfterRemove (IDbContext dbContext, IListItem obj);
		public void UpdateAfterInsert (IDbContext dbContext, IListItem? target, IListItem input);
		public void UpdateAfterReorder (IDbContext dbContext, IListItem source, IListItem target);

		/// <summary>
		/// output a new sorted list based on the source
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public IEnumerable<T> SortListObjects<T> (IList<T> source) where T : IListItem;

		/// <summary>
		/// output an enumerable of dictionary objects sorted by the value of the key-value pair
		/// </summary>
		/// <param name="dictionary"></param>
		/// <returns></returns>
		public IEnumerable<KeyValuePair<IListItem, int>> SortDictionaryObjects (
			IReadOnlyDictionary<IListItem, int> dictionary);
	}
}

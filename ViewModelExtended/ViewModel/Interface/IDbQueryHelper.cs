using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IDbQueryHelper
	{
		public IQueryable<NoteListObjectViewModel> GetAllNoteListObjects (IDbContext dbContext);
		public IQueryable<GroupListObjectViewModel> GetAllGroupListObjects (IDbContext dbContext);

		public IQueryable<Tuple<GroupItem, ObjectRoot>> GetGroupItemsInGroup (IDbContext dbContext, Group groop);

		public IQueryable<GroupObjectViewModel> GetGroupObjectsInGroup (IDbContext dbContext, Group groop);
		public IQueryable<GroupObjectViewModel> GetGroupObjectsByNote (IDbContext dbContext, Note note);

		/// <summary>
		/// sort a dictionary with an integer value
		/// </summary>
		/// <param name="listItems"></param>
		/// <returns></returns>
		public IEnumerable<KeyValuePair<T, int>> SortDictionary<T> (Dictionary<T, int> listItems) where T : IListItem;

		/// <summary>
		/// sort a basic list into an IObservableList
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void GetSortedListObjects<T> (IList<T> source, IObservableList<T> target) where T : IListItem;
	}
}

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

		public IQueryable<KeyValuePair<GroupItem, ObjectRoot>> GetGroupItemsInGroup (IDbContext dbContext, Group groop);

		public IQueryable<GroupObjectViewModel> GetGroupObjectsInGroup (IDbContext dbContext, Group groop);
		public IQueryable<GroupObjectViewModel> GetGroupObjectsByNote (IDbContext dbContext, Note note);
		//public void GetSortedListObjects<T> (IQueryable<T> source, IObservableList<T> target) where T : IListItem;
		public void GetSortedListObjects<T> (IList<T> source, IObservableList<T> target) where T : IListItem;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IDbQueryHelper
	{
		public IQueryable<IListItem> GetAllNoteListObjects (IDbContext dbContext);
		public IQueryable<IListItem> GetAllGroupListObjects (IDbContext dbContext);
		public IQueryable<IListItem> GetGroupObjectsInGroup (IDbContext dbContext, Group groop);
		public void GetSortedListObjects (IQueryable<IListItem> source, IObservableList target);
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IDbListHelper
	{
		public void UpdateNodes (IDbContext dbContext, IListItem? item);

		public void UpdateAfterAdd (IDbContext dbContext, IListItem item);
		public void UpdateAfterRemove (IDbContext dbContext, IListItem obj);
		public void UpdateAfterInsert (IDbContext dbContext, IListItem? target, IListItem input);
		public void UpdateAfterReorder (IDbContext dbContext, IListItem source, IListItem target);
	}
}

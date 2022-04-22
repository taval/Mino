using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IDbHelper
	{
		public void UpdateNodes (IDbContext dbContext, IListItem? item);
	}
}

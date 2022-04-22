using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class DbHelper : IDbHelper
	{
		public void UpdateNodes (IDbContext dbContext, IListItem? item)
		{
			if (item != null) {
				// update database
				IListItem current = item;
				IListItem? previous = item.Previous;
				IListItem? next = item.Next;

				dbContext.UpdateNode(current.Node, previous?.Node, next?.Node);
			}
		}
	}
}

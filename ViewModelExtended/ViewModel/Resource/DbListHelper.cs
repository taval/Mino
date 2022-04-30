using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class DbListHelper : IDbListHelper
	{
		public void UpdateAfterAdd (IDbContext dbContext, IListItem input)
		{
			UpdateNodes(dbContext, input);
			UpdateNodes(dbContext, input.Previous);
			UpdateNodes(dbContext, input.Next);
		}

		public void UpdateAfterInsert (IDbContext dbContext, IListItem? target, IListItem input)
		{
			UpdateNodes(dbContext, target);
			UpdateNodes(dbContext, input);
		}

		public void UpdateAfterReorder (IDbContext dbContext, IListItem source, IListItem target)
		{
			UpdateNodes(dbContext, source);
			UpdateNodes(dbContext, target);
		}

		public void UpdateAfterRemove (IDbContext dbContext, IListItem input)
		{
			UpdateNodes(dbContext, input.Previous);
			UpdateNodes(dbContext, input.Next);
		}

		public void UpdateNodes (IDbContext dbContext, IListItem? item)
		{
			if (item != null) {
				// update database
				IListItem current = item;
				IListItem? previous = item.Previous;
				IListItem? next = item.Next;

				dbContext.UpdateNode((Node)current.Node, (Node?)previous?.Node, (Node?)next?.Node);
			}
		}
	}
}

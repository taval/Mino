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

				dbContext.UpdateNode(current.Node, previous?.Node, next?.Node);
			}
		}

		/// <summary>
		/// create the relationship between Notes in a linked list
		/// assumes note begin/end nodes will correctly contain null upon immediate construction
		/// </summary>
		/// <param name="items"></param>
		public static void LinkAll (IEnumerable<IListItem> items)
		{
			IListItem? prev = null;
			IListItem? current = null;
			IListItem? next = null;

			foreach (IListItem note in items) {
				// set first node
				if (prev == null) {
					prev = note;
					continue;
				}

				// set second node
				if (prev != null && current == null) {
					current = note;
					continue;
				}

				// connect subsequent nodes
				if (prev != null && current != null) {
					next = note;
					prev.Next = current;
					current.Previous = prev;
					current.Next = next;
					next.Previous = current;
					prev = current;
					current = next;
				}
			}
		}
	}
}

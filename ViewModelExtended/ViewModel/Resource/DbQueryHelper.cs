using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class DbQueryHelper : IDbQueryHelper
	{
		private IViewModelResource Resource { get; set; }



		#region Constructor

		public DbQueryHelper (IViewModelResource resource)
		{
			Resource = resource;
		}

		#endregion



		#region NoteList

		public IQueryable<IListItem> GetAllNoteListObjects (IDbContext dbContext)
		{
			return from item in dbContext.GetAllNoteListItems()
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				select Resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext.CreateNoteListObject(item, dbContext.CreateObjectRoot(node, timestamp), data));
		}

		#endregion



		#region GroupList

		public IQueryable<IListItem> GetAllGroupListObjects (IDbContext dbContext)
		{
			return from item in dbContext.GetAllGroupListItems()
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Groups on item.ObjectId equals data.Id
				select Resource.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext.CreateGroupListObject(item, dbContext.CreateObjectRoot(node, timestamp), data));
		}

		#endregion



		#region Group

		/// <summary>
		/// experimental: associate NoteListObjects with each GroupObject
		/// </summary>
		/// <param name="dbContext"></param>
		/// <returns></returns>
		//public IQueryable<IListItem> GetGroupObjectsInGroup (
		//	IDbContext dbContext, IEnumerable<IListItem> notes, Group groop)
		//{
		//	return from item in dbContext.GetGroupItemsInGroup(groop)
		//		join node in dbContext.Nodes on item.NodeId equals node.Id
		//		join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
		//		join noteListViewModel in notes on item.ObjectId equals noteListViewModel.DataId
		//		select Resource.ViewModelCreator.CreateGroupObjectViewModel(
		//			dbContext.CreateGroupObject(
		//				item, dbContext.CreateObjectRoot(node, timestamp), groop, noteListViewModel));
		//}

		public IQueryable<IListItem> GetGroupObjectsInGroup (IDbContext dbContext, Group groop)
		{
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		public IQueryable<IListItem> GetGroupObjectsByNote (IDbContext dbContext, Note note)
		{
			return from item in dbContext.GetAllGroupItems()
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				join groop in dbContext.Groups on item.GroupId equals groop.Id
				where data.Id == note.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		#endregion



		#region Sort

		public void GetSortedListObjects (IQueryable<IListItem> source, IObservableList target)
		{
			target.Clear();
			List<IListItem> objectList = source.ToList();

			// first object
			IListItem? firstObject = objectList.Find(obj => obj.Node.PreviousId == null);

			if (firstObject == null) {
				return;
			}

			target.Add(firstObject);
			objectList.Remove(firstObject);

			IListItem? currentObject = firstObject;
			IListItem? nextObject = null;

			// remaining objects
			while (objectList.Count > 0) {
				nextObject = objectList.Find(obj => obj.Node.Id == currentObject.Node.NextId);
				currentObject.Next = nextObject;

				if (nextObject == null) {
					return;
				}

				nextObject.Previous = currentObject;

				target.Add(nextObject);
				objectList.Remove(nextObject);

				currentObject = nextObject;
			}
		}

		#endregion
	}
}

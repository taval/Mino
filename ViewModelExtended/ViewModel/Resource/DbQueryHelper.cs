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

		public IQueryable<IListItem> GetAllGroupObjects (IDbContext dbContext)
		{
			return from item in dbContext.GetAllGroupItems()
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				join groop in dbContext.Groups on item.GroupId equals groop.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		public IQueryable<IListItem> GetGroupObjectsInGroup (IDbContext dbContext, Group groop)
		{
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		public IQueryable<IListItem> GetGroupObjectByNodeId (IDbContext dbContext, Group groop, int? nodeId)
		{
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join fullNode in dbContext.Nodes on item.NodeId equals fullNode.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				where fullNode.Id == nodeId
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(fullNode, timestamp), groop, data));
		}

		public IEnumerable<IListItem> GetGroupObjectsOfNote (IDbContext dbContext, NoteListObjectViewModel target)
		{
			IQueryable<IListItem> groupObjects = Resource.DbQueryHelper.GetAllGroupObjects(dbContext);

			Action<GroupObjectViewModel> action = (obj) =>
			{
				// get previous GroupObject
				IQueryable<IListItem> previous = Resource.DbQueryHelper.GetGroupObjectByNodeId(
					dbContext, obj.Model.Group, obj.Model.Node.PreviousId);

				if (previous.Count() > 0) obj.Previous = previous?.First();

				// get next GroupObject
				IQueryable<IListItem> next = Resource.DbQueryHelper.GetGroupObjectByNodeId(
					dbContext, obj.Model.Group, obj.Model.Node.NextId);

				if (next.Count() > 0) obj.Next = next?.First();
			};

			// get all GroupObjects matching the NoteListObject
			return
				from groupObj in groupObjects
				where ((GroupObjectViewModel)groupObj).Model.Data.Id == target.Model.Data.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					((GroupObjectViewModel)groupObj).Model, action);
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

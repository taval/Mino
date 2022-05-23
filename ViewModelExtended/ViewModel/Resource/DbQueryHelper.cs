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

		public IQueryable<NoteListObjectViewModel> GetAllNoteListObjects (IDbContext dbContext)
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

		public IQueryable<GroupListObjectViewModel> GetAllGroupListObjects (IDbContext dbContext)
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
		/// experimental: associate NoteListObjects with each GroupObject client-side - just load the base objects here
		/// the end group is known client-side so not passing along to create the full GroupObject should be ok
		/// </summary>
		/// <param name="dbContext"></param>
		/// <returns></returns>
		public IQueryable<Tuple<GroupItem, ObjectRoot>> GetGroupItemsInGroup (IDbContext dbContext, Group groop)
		{
			//dbContext.CreateGroupObject(item, root, groop, data)
			//Resource.ViewModelCreator.CreateGroupObjectViewModel(
			//join data in dbContext.Notes on item.ObjectId equals data.Id
			//join node in dbContext.Nodes on item.NodeId equals node.Id
			//	join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
			//	select dbContext.CreateObjectRoot(node, timestamp);
			//dbContext.GetGroupItemsInGroup(groop);
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				select new Tuple<GroupItem, ObjectRoot>(item, dbContext.CreateObjectRoot(node, timestamp));
		}

		public IQueryable<GroupObjectViewModel> GetGroupObjectsInGroup (IDbContext dbContext, Group groop)
		{
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join node in dbContext.Nodes on item.NodeId equals node.Id
				join timestamp in dbContext.Timestamps on item.TimestampId equals timestamp.Id
				join data in dbContext.Notes on item.ObjectId equals data.Id
				select Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		public IQueryable<GroupObjectViewModel> GetGroupObjectsByNote (IDbContext dbContext, Note note)
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

		public void GetUnsortedListObjects<T> (IList<T> source, IObservableList<T> target) where T : IListItem
		{
			target.Clear();

			foreach (T obj in source) target.Add(obj);
		}

		public void GetSortedListObjects<T> (IList<T> source, IObservableList<T> target) where T : IListItem
		{
			target.Clear();

			IList<T> sourceCopy = source.ToList();

			// first object
			IEnumerable<T> first = sourceCopy.Where(obj => obj.Node.PreviousId == null);

			if (!first.Any()) return;
			T firstObject = first.Single();

			target.Add(firstObject);
			sourceCopy.Remove(firstObject);

			T currentObject = firstObject;

			// remaining objects
			while (sourceCopy.Count > 0) {
				IEnumerable<T> next = sourceCopy.Where(obj => obj.Node.Id == currentObject.Node.NextId);

				if (!next.Any()) {
					currentObject.Next = null;
					return;
				}
				T nextObject = next.Single();
				currentObject.Next = nextObject;

				nextObject.Previous = currentObject;

				target.Add(nextObject);
				sourceCopy.Remove(nextObject);

				currentObject = nextObject;
			}
		}

		#endregion
	}
}

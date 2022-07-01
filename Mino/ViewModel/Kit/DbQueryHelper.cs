using System;
using System.Linq;
using Mino.Model;



namespace Mino.ViewModel
{
	public class DbQueryHelper : IDbQueryHelper
	{
		#region Kit

		private IViewModelCreator f_ViewModelCreator;

		#endregion



		#region Constructor

		public DbQueryHelper (IViewModelCreator viewModelCreator)
		{
			f_ViewModelCreator = viewModelCreator;
		}

		#endregion



		#region NoteList

		public IQueryable<NoteListObjectViewModel> GetAllNoteListObjects (IDbContext dbContext)
		{
			return from item in dbContext.GetAllNoteListItems()
				join node in dbContext.GetAllNodes() on item.NodeId equals node.Id
				join timestamp in dbContext.GetAllTimestamps() on item.TimestampId equals timestamp.Id
				join data in dbContext.GetAllNotes() on item.ObjectId equals data.Id
				select f_ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext.CreateNoteListObject(item, dbContext.CreateObjectRoot(node, timestamp), data));
		}

		#endregion



		#region GroupList

		public IQueryable<GroupListObjectViewModel> GetAllGroupListObjects (IDbContext dbContext)
		{
			return from item in dbContext.GetAllGroupListItems()
				join node in dbContext.GetAllNodes() on item.NodeId equals node.Id
				join timestamp in dbContext.GetAllTimestamps() on item.TimestampId equals timestamp.Id
				join data in dbContext.GetAllGroups() on item.ObjectId equals data.Id
				select f_ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext.CreateGroupListObject(item, dbContext.CreateObjectRoot(node, timestamp), data));
		}

		#endregion



		#region Group

		public IQueryable<Tuple<GroupItem, ObjectRoot>> GetGroupItemsInGroup (IDbContext dbContext, Group groop)
		{
			return from item in dbContext.GetGroupItemsInGroup(groop)
				join node in dbContext.GetAllNodes() on item.NodeId equals node.Id
				join timestamp in dbContext.GetAllTimestamps() on item.TimestampId equals timestamp.Id
				join data in dbContext.GetAllNotes() on item.ObjectId equals data.Id
				select new Tuple<GroupItem, ObjectRoot>(item, dbContext.CreateObjectRoot(node, timestamp));
		}

		public IQueryable<GroupObjectViewModel> GetGroupObjectsByNote (IDbContext dbContext, Note note)
		{
			return from item in dbContext.GetAllGroupItems()
				join node in dbContext.GetAllNodes() on item.NodeId equals node.Id
				join timestamp in dbContext.GetAllTimestamps() on item.TimestampId equals timestamp.Id
				join data in dbContext.GetAllNotes() on item.ObjectId equals data.Id
				join groop in dbContext.GetAllGroups() on item.GroupId equals groop.Id
				where data.Id == note.Id
				select f_ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item, dbContext.CreateObjectRoot(node, timestamp), groop, data));
		}

		#endregion
	}
}

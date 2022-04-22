using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class ViewModelCreator : IViewModelCreator
	{
		#region Resource

		private IViewModelResource Resource { get; set; }

		#endregion



		#region Constructor

		public ViewModelCreator (IViewModelResource resource)
		{
			Resource = resource;
		}

		#endregion



		#region NoteList

		public NoteListViewModel CreateNoteListViewModel ()
		{
			return new NoteListViewModel(Resource);
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// create basic data components
				Note note = dbContext.CreateNote("", "");
				INode node = dbContext.CreateNode(null, null);
				Timestamp timestamp = dbContext.CreateTimestamp();
				dbContext.Save();

				// create root object
				IObject root = dbContext.CreateObjectRoot(node, timestamp);

				// create item context
				NoteListItem item = dbContext.CreateNoteListItem(root, note);
				dbContext.Save();

				// create instance wrapper
				NoteListObject obj = dbContext.CreateNoteListObject(item, root, note);

				return new NoteListObjectViewModel(obj);
			}
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (Action<NoteListObjectViewModel> action)
		{
			NoteListObjectViewModel output = CreateNoteListObjectViewModel();

			action(output);

			return output;
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (NoteListObject data)
		{
			return new NoteListObjectViewModel(data);
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (NoteListObject data, Action<NoteListObjectViewModel> action)
		{
			NoteListObjectViewModel output = CreateNoteListObjectViewModel(data);

			action(output);

			return output;
		}

		#endregion



		#region GroupList

		public GroupListViewModel CreateGroupListViewModel ()
		{
			return new GroupListViewModel(Resource);
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// create basic data components
				Group data = dbContext.CreateGroup("", "#999");
				INode node = dbContext.CreateNode(null, null);
				Timestamp timestamp = dbContext.CreateTimestamp();
				dbContext.Save();

				// create root object
				IObject root = dbContext.CreateObjectRoot(node, timestamp);

				// create item context
				GroupListItem item = dbContext.CreateGroupListItem(root, data);
				dbContext.Save();

				// create instance wrapper
				GroupListObject obj = dbContext.CreateGroupListObject(item, root, data);

				return new GroupListObjectViewModel(obj);
			}
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (Action<GroupListObjectViewModel> action)
		{
			GroupListObjectViewModel output = CreateGroupListObjectViewModel();

			action(output);

			return output;
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (GroupListObject data)
		{
			return new GroupListObjectViewModel(data);
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (GroupListObject data, Action<GroupListObjectViewModel> action)
		{
			GroupListObjectViewModel output = CreateGroupListObjectViewModel(data);

			action(output);

			return output;
		}

		#endregion



		#region Group

		//public GroupContentsViewModel CreateGroupContentsViewModel (Group groop)
		//{
		//	return new GroupContentsViewModel(Resource, groop);
		//}

		public GroupContentsViewModel CreateGroupContentsViewModel ()
		{
			return new GroupContentsViewModel(Resource);
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (Group groop, Note data)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// create basic data components
				INode node = dbContext.CreateNode(null, null);
				Timestamp timestamp = dbContext.CreateTimestamp();
				dbContext.Save();

				// create root object
				IObject root = dbContext.CreateObjectRoot(node, timestamp);

				// create item context
				GroupItem item = dbContext.CreateGroupItem(root, groop, data);
				dbContext.Save();

				// create instance wrapper
				GroupObject obj = dbContext.CreateGroupObject(item, root, groop, data);

				return new GroupObjectViewModel(obj);
			}
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (Group groop, Note note, Action<GroupObjectViewModel> action)
		{
			GroupObjectViewModel output = CreateGroupObjectViewModel(groop, note);

			action(output);

			return output;
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (GroupObject data)
		{
			return new GroupObjectViewModel(data);
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (GroupObject data, Action<GroupObjectViewModel> action)
		{
			GroupObjectViewModel output = CreateGroupObjectViewModel(data);

			action(output);

			return output;
		}

		#endregion



		#region NoteText

		public NoteTextViewModel CreateNoteTextViewModel ()
		{
			return new NoteTextViewModel(Resource);
		}

		#endregion



		#region GroupTabs

		public GroupTabsViewModel CreateGroupTabsViewModel ()
		{
			return new GroupTabsViewModel(Resource);
		}

		#endregion



		#region PrimeViewModel

		public PrimeViewModel CreatePrimeViewModel ()
		{
			return new PrimeViewModel(Resource);
		}

		#endregion



		#region MainWindow

		public MainWindowViewModel CreateMainWindowViewModel ()
		{
			return new MainWindowViewModel(Resource);
		}

		#endregion



		#region Destructors

		public void DestroyMainWindowViewModel (MainWindowViewModel target)
		{
			// nothing to do
		}

		public void DestroyPrimeViewModel (PrimeViewModel target)
		{
			// nothing to do
		}

		public void DestroyGroupTabsViewModel (GroupTabsViewModel target)
		{
			// nothing to do
		}

		public void DestroyNoteListViewModel (NoteListViewModel target)
		{
			// nothing to do
		}

		public void DestroyGroupListViewModel (GroupListViewModel target)
		{
			// nothing to do
		}

		public void DestroyGroupContentsViewModel (GroupContentsViewModel target)
		{
			// nothing to do

		}

		public void DestroyNoteTextViewModel (NoteTextViewModel target)
		{
			// nothing to do
		}

		public void DestroyNoteListObjectViewModel (NoteListObjectViewModel target)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.DeleteNoteListObject(target.Data);
			}
		}

		public void DestroyGroupListObjectViewModel (GroupListObjectViewModel target)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.DeleteGroupListObject(target.Data);
			}
		}

		public void DestroyGroupObjectViewModel (GroupObjectViewModel target)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.DeleteGroupObject(target.Data);
			}
		}

		#endregion

	}
}

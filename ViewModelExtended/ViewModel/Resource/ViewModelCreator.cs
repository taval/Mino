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

		public NoteListObjectViewModel CreateNoteListObjectViewModel (IDbContext dbContext)
		{
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

			// create model instance wrapper
			NoteListObject model = dbContext.CreateNoteListObject(item, root, note);

			return new NoteListObjectViewModel(model);
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (
			IDbContext dbContext, Action<NoteListObjectViewModel> action)
		{
			NoteListObjectViewModel output = CreateNoteListObjectViewModel(dbContext);

			// save the external modifications
			action(output);
			dbContext.UpdateNoteListObject(output.Model);
			dbContext.Save();

			return output;
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (NoteListObject model)
		{
			return new NoteListObjectViewModel(model);
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (
			NoteListObject model, IDbContext dbContext, Action<NoteListObjectViewModel> action)
		{
			NoteListObjectViewModel output = CreateNoteListObjectViewModel(model);

			// save the external modifications
			action(output);
			dbContext.UpdateNoteListObject(output.Model);
			dbContext.Save();

			return output;
		}

		#endregion



		#region GroupList

		public GroupListViewModel CreateGroupListViewModel ()
		{
			return new GroupListViewModel(Resource);
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (IDbContext dbContext)
		{
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

			// create model instance wrapper
			GroupListObject model = dbContext.CreateGroupListObject(item, root, data);

			return new GroupListObjectViewModel(model);
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (
			IDbContext dbContext, Action<GroupListObjectViewModel> action)
		{
			GroupListObjectViewModel output = CreateGroupListObjectViewModel(dbContext);

			// save the external modifications
			action(output);
			dbContext.UpdateGroupListObject(output.Model);
			dbContext.Save();

			return output;
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (GroupListObject model)
		{
			return new GroupListObjectViewModel(model);
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (
			GroupListObject model, IDbContext dbContext, Action<GroupListObjectViewModel> action)
		{
			GroupListObjectViewModel output = CreateGroupListObjectViewModel(model);

			// save the external modifications
			action(output);
			dbContext.UpdateGroupListObject(output.Model);
			dbContext.Save();

			return output;
		}

		#endregion



		#region Group

		public GroupContentsViewModel CreateGroupContentsViewModel ()
		{
			return new GroupContentsViewModel(Resource);
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (IDbContext dbContext, Group groop, Note data)
		{
			// create basic data components
			INode node = dbContext.CreateNode(null, null);
			Timestamp timestamp = dbContext.CreateTimestamp();
			dbContext.Save();

			// create root object
			IObject root = dbContext.CreateObjectRoot(node, timestamp);

			// create item context
			GroupItem item = dbContext.CreateGroupItem(root, groop, data);
			dbContext.Save();

			// create model instance wrapper
			GroupObject model = dbContext.CreateGroupObject(item, root, groop, data);

			return new GroupObjectViewModel(model);
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (
			IDbContext dbContext, Group groop, Note note, Action<GroupObjectViewModel> action)
		{
			GroupObjectViewModel output = CreateGroupObjectViewModel(dbContext, groop, note);

			// save the external modifications
			action(output);
			dbContext.UpdateGroupObject(output.Model);
			dbContext.Save();

			return output;
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (GroupObject model)
		{
			return new GroupObjectViewModel(model);
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (
			GroupObject model, IDbContext dbContext, Action<GroupObjectViewModel> action)
		{
			GroupObjectViewModel output = CreateGroupObjectViewModel(model);

			// save the external modifications
			action(output);
			dbContext.UpdateGroupObject(output.Model);
			dbContext.Save();

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

		public void DestroyNoteListObjectViewModel (IDbContext dbContext, NoteListObjectViewModel target)
		{
			dbContext.DeleteNoteListObject(target.Model);
			dbContext.Save();
		}

		public void DestroyGroupListObjectViewModel (IDbContext dbContext, GroupListObjectViewModel target)
		{
			dbContext.DeleteGroupListObject(target.Model);
			dbContext.Save();
		}

		public void DestroyGroupObjectViewModel (IDbContext dbContext, GroupObjectViewModel target)
		{
			dbContext.DeleteGroupObject(target.Model);
			dbContext.Save();
		}

		public IObservableList CreateList ()
		{
			return new ObservableList();
		}

		#endregion

	}
}

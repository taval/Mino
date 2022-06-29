using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Mino.Model;

// TODO: (do search for all references of Model here)
// exposing the model(s) directly allows opportunities for breaking data integrity. 
// maybe UpdateGroupObject could call a function on GroupObjectViewModel that performs the update somehow? perhaps UpdateGroupObject's signature should be changed to raw data parameters, OR it belongs elsewhere as a function separate from dbContext, with dbContext as a dependency in its signature
// OR the model is swapped out for an interface, and the implementation that is sent to GroupObjectViewModel is read-only members

namespace Mino.ViewModel
{
	public class ViewModelCreator : IViewModelCreator
	{
		#region Kit

		private IViewModelKit f_ViewModelKit;

		#endregion



		//#region Color Generator

		//private ColorGenerator f_ColorGenerator;

		//#endregion



		#region Constructor

		public ViewModelCreator (IViewModelKit viewModelKit)
		{
			f_ViewModelKit = viewModelKit;

			//f_ColorGenerator = new ColorGenerator();
		}

		#endregion



		#region State

		public StateViewModel CreateStateViewModel ()
		{
			StateViewModel output = new StateViewModel(f_ViewModelKit);
			// TODO: any commands on StateViewModel are assigned here
			return output;
		}

		#endregion



		#region NoteList

		public NoteListViewModel CreateNoteListViewModel ()
		{
			NoteListViewModel output = new NoteListViewModel(f_ViewModelKit);
			f_ViewModelKit.CommandBuilder.MakeNoteList(output);
			return output;
		}

		public NoteListObjectViewModel CreateNoteListObjectViewModel (IDbContext dbContext)
		{
			// create basic data components
			Note note = dbContext.CreateNote("", "", 0);
			Node node = dbContext.CreateNode(null, null);
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
			GroupListViewModel output = new GroupListViewModel(f_ViewModelKit);
			f_ViewModelKit.CommandBuilder.MakeGroupList(output);
			return output;
		}

		public GroupListObjectViewModel CreateGroupListObjectViewModel (IDbContext dbContext)
		{
			// create basic data components
			Group data = dbContext.CreateGroup("", "#999");
			//Color newColor = f_ColorGenerator.Generate();
			//Group data = dbContext.CreateGroup("", ColorTranslator.ToHtml(newColor));
			Node node = dbContext.CreateNode(null, null);
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

		public GroupContentsViewModel CreateGroupContentsViewModel (NoteListViewModel noteListViewModel)
		{
			GroupContentsViewModel output = new GroupContentsViewModel(f_ViewModelKit, noteListViewModel);
			f_ViewModelKit.CommandBuilder.MakeGroup(output);
			return output;
		}

		public GroupObjectViewModel CreateGroupObjectViewModel (IDbContext dbContext, Group groop, Note data)
		{
			// create basic data components
			Node node = dbContext.CreateNode(null, null);
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

		public GroupObjectViewModel CreateTempGroupObjectViewModel (IDbContext dbContext, Group groop, Note data)
		{
			// create basic data components
			Node node = dbContext.CreateNode(null, null, true);
			//dbContext.Entry(node).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

			Timestamp timestamp = dbContext.CreateTimestamp(true);

			// create root object
			IObject root = dbContext.CreateObjectRoot(node, timestamp);

			// create item context
			GroupItem item = dbContext.CreateGroupItem(root, groop, data, true);

			// create model instance wrapper
			GroupObject model = dbContext.CreateGroupObject(item, root, groop, data);

			return new GroupObjectViewModel(model);
		}

		#endregion



		#region NoteText

		public NoteTextViewModel CreateNoteTextViewModel (StateViewModel stateViewModel)
		{
			NoteTextViewModel output = new NoteTextViewModel(f_ViewModelKit, stateViewModel);
			f_ViewModelKit.CommandBuilder.MakeNoteText(output);
			return output;
		}

		#endregion



		#region GroupTabs

		public GroupTabsViewModel CreateGroupTabsViewModel (
			StateViewModel stateViewModel,
			GroupListViewModel groupListViewModel,
			GroupContentsViewModel groupContentsViewModel)
		{
			GroupTabsViewModel output =
				new GroupTabsViewModel(stateViewModel, groupListViewModel, groupContentsViewModel);

			f_ViewModelKit.CommandBuilder.MakeGroupTabs(output);

			return output;
		}

		#endregion



		#region StatusBar

		public StatusBarViewModel CreateStatusBarViewModel ()
		{
			return new StatusBarViewModel();
		}

		#endregion



		#region PrimeViewModel

		public PrimeViewModel CreatePrimeViewModel (
			StateViewModel stateViewModel,
			StatusBarViewModel statusBarViewModel,
			NoteTextViewModel noteTextViewModel,
			GroupTabsViewModel groupTabsViewModel,
			NoteListViewModel noteListViewModel)
		{
			//PrimeViewModel output = new PrimeViewModel(
			//	f_ViewModelKit, statusBarViewModel, noteTextViewModel, groupTabsViewModel, noteListViewModel);
			PrimeViewModel output = new PrimeViewModel(
				stateViewModel, statusBarViewModel, noteTextViewModel, groupTabsViewModel, noteListViewModel);
			f_ViewModelKit.CommandBuilder.MakePrime(output);
			return output;
		}

		#endregion



		#region MainWindow

		public MainWindowViewModel CreateMainWindowViewModel ()
		{
			//MainWindowViewModel output = new MainWindowViewModel(f_ViewModelKit);
			MainWindowViewModel output = new MainWindowViewModel();
			f_ViewModelKit.CommandBuilder.MakeMainWindow(output);
			return output;
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

		public void DestroyStatusBarViewModel (StatusBarViewModel target)
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

		#endregion

	}
}

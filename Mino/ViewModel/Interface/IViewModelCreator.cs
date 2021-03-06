using System;
using Mino.Model;



namespace Mino.ViewModel
{
	public interface IViewModelCreator
	{
		#region State

		public StateViewModel CreateStateViewModel ();

		#endregion

		// ListObjects have two input methods: new data and existing data

		#region NoteList

		public NoteListViewModel CreateNoteListViewModel ();

		// new
		public NoteListObjectViewModel CreateNoteListObjectViewModel (IDbContext dbContext);
		public NoteListObjectViewModel CreateNoteListObjectViewModel (
			IDbContext dbContext, Action<NoteListObjectViewModel> action);

		// existing
		public NoteListObjectViewModel CreateNoteListObjectViewModel (NoteListObject data);
		public NoteListObjectViewModel CreateNoteListObjectViewModel (
			NoteListObject data, IDbContext dbContext, Action<NoteListObjectViewModel> action);

		// destroy
		public void DestroyNoteListObjectViewModel (IDbContext dbContext, NoteListObjectViewModel target);
		public void DestroyNoteListViewModel (NoteListViewModel target);

		#endregion



		#region GroupList

		public GroupListViewModel CreateGroupListViewModel ();

		// new
		public GroupListObjectViewModel CreateGroupListObjectViewModel (IDbContext dbContext);
		public GroupListObjectViewModel CreateGroupListObjectViewModel (
			IDbContext dbContext, Action<GroupListObjectViewModel> action);

		// existing
		public GroupListObjectViewModel CreateGroupListObjectViewModel (GroupListObject data);
		public GroupListObjectViewModel CreateGroupListObjectViewModel (
			GroupListObject data, IDbContext dbContext, Action<GroupListObjectViewModel> action);

		// destroy
		public void DestroyGroupListObjectViewModel (IDbContext dbContext, GroupListObjectViewModel target);
		public void DestroyGroupListViewModel (GroupListViewModel target);

		#endregion



		#region GroupContents

		public GroupContentsViewModel CreateGroupContentsViewModel (NoteListViewModel noteListViewModel);

		// new
		// note there is no default for a GroupObject because it is dependent on an existing Group and specific Note
		public GroupObjectViewModel CreateGroupObjectViewModel (IDbContext dbContext, Group groop, Note note);
		public GroupObjectViewModel CreateGroupObjectViewModel (
			IDbContext dbContext, Group groop, Note note, Action<GroupObjectViewModel> action);

		// existing
		public GroupObjectViewModel CreateGroupObjectViewModel (GroupObject data);
		public GroupObjectViewModel CreateGroupObjectViewModel (
			GroupObject data, IDbContext dbContext, Action<GroupObjectViewModel> action);

		// temp
		public GroupObjectViewModel CreateTempGroupObjectViewModel (IDbContext dbContext, Group groop, Note note);

		// destroy
		public void DestroyGroupObjectViewModel (IDbContext dbContext, GroupObjectViewModel target);
		public void DestroyGroupContentsViewModel (GroupContentsViewModel target);

		#endregion



		#region NoteText

		public NoteTextViewModel CreateNoteTextViewModel (StateViewModel stateViewModel);
		public void DestroyNoteTextViewModel (NoteTextViewModel target);

		#endregion



		#region GroupTabs

		public GroupTabsViewModel CreateGroupTabsViewModel (
			StateViewModel stateViewModel,
			GroupListViewModel groupListViewModel,
			GroupContentsViewModel groupContentsViewModel);

		public void DestroyGroupTabsViewModel (GroupTabsViewModel target);

		#endregion



		#region StatusBar



		#endregion

		public StatusBarViewModel CreateStatusBarViewModel ();
		public void DestroyStatusBarViewModel (StatusBarViewModel target);

		#region Prime

		public PrimeViewModel CreatePrimeViewModel (
			StateViewModel stateViewModel,
			StatusBarViewModel statusBarViewModel,
			NoteTextViewModel noteTextViewModel,
			GroupTabsViewModel groupTabsViewModel,
			NoteListViewModel noteListViewModel);
		public void DestroyPrimeViewModel (PrimeViewModel target);

		#endregion



		#region MainWindow

		public MainWindowViewModel CreateMainWindowViewModel ();
		public void DestroyMainWindowViewModel (MainWindowViewModel target);

		#endregion
	}
}

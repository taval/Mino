using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IViewModelCreator
	{
		// ListObjects have two input methods: new data and existing data

		#region NoteList

		public NoteListViewModel CreateNoteListViewModel ();

		// new
		public NoteListObjectViewModel CreateNoteListObjectViewModel ();
		public NoteListObjectViewModel CreateNoteListObjectViewModel (Action<NoteListObjectViewModel> action);

		// existing
		public NoteListObjectViewModel CreateNoteListObjectViewModel (NoteListObject data);
		public NoteListObjectViewModel CreateNoteListObjectViewModel (
			NoteListObject data, Action<NoteListObjectViewModel> action);

		// destroy
		public void DestroyNoteListObjectViewModel (NoteListObjectViewModel target);
		public void DestroyNoteListViewModel (NoteListViewModel target);

		#endregion



		#region GroupList

		public GroupListViewModel CreateGroupListViewModel ();

		// new
		public GroupListObjectViewModel CreateGroupListObjectViewModel ();
		public GroupListObjectViewModel CreateGroupListObjectViewModel (Action<GroupListObjectViewModel> action);

		// existing
		public GroupListObjectViewModel CreateGroupListObjectViewModel (GroupListObject data);
		public GroupListObjectViewModel CreateGroupListObjectViewModel (
			GroupListObject data, Action<GroupListObjectViewModel> action);

		// destroy
		public void DestroyGroupListObjectViewModel (GroupListObjectViewModel target);
		public void DestroyGroupListViewModel (GroupListViewModel target);

		#endregion



		#region GroupContents

		//public GroupContentsViewModel CreateGroupContentsViewModel (Group groop);
		public GroupContentsViewModel CreateGroupContentsViewModel (); // no group selected by default

		// new
		// note there is no default for a GroupObject because it is dependent on an existing Group and specific Note
		public GroupObjectViewModel CreateGroupObjectViewModel (Group groop, Note note);
		public GroupObjectViewModel CreateGroupObjectViewModel (
			Group groop, Note note, Action<GroupObjectViewModel> action);

		// existing
		public GroupObjectViewModel CreateGroupObjectViewModel (GroupObject data);
		public GroupObjectViewModel CreateGroupObjectViewModel (
			GroupObject data, Action<GroupObjectViewModel> action);

		// destroy
		public void DestroyGroupObjectViewModel (GroupObjectViewModel target);
		public void DestroyGroupContentsViewModel (GroupContentsViewModel target);

		#endregion



		#region NoteText

		public NoteTextViewModel CreateNoteTextViewModel ();
		public void DestroyNoteTextViewModel (NoteTextViewModel target);

		#endregion



		#region GroupTabs

		public GroupTabsViewModel CreateGroupTabsViewModel ();
		public void DestroyGroupTabsViewModel (GroupTabsViewModel target);

		#endregion



		#region Prime

		public PrimeViewModel CreatePrimeViewModel ();
		public void DestroyPrimeViewModel (PrimeViewModel target);

		#endregion



		#region MainWindow

		public MainWindowViewModel CreateMainWindowViewModel ();
		public void DestroyMainWindowViewModel (MainWindowViewModel target);

		#endregion
	}
}

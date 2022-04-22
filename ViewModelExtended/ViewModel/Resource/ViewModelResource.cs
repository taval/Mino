using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class ViewModelResource : IViewModelResource
	{
		public IDbHelper DbHelper { get; private set; }
		public IDbQueryHelper DbQueryHelper { get; private set; }
		public IViewModelCreator ViewModelCreator { get; private set; }
		public ICommandBuilder CommandBuilder { get; private set; }

		public MainWindowViewModel MainWindowViewModel { get; private set; }
		public PrimeViewModel PrimeViewModel { get; private set; }
		public GroupTabsViewModel GroupTabsViewModel { get; private set; }
		public NoteListViewModel NoteListViewModel { get; private set; }
		public GroupListViewModel GroupListViewModel { get; private set; }
		public GroupContentsViewModel GroupContentsViewModel { get; private set; }
		public NoteTextViewModel NoteTextViewModel { get; private set; }

		public ViewModelResource ()
		{
			// TODO: this is logically dependent on the order of construction:
			//       NoteText is dependent on NoteList
			//       GroupContents is dependent on GroupList
			//       PrimeViewModel is dependent on NoteText/NoteList and GroupContents/GroupList
			//       all are dependent on the helpers and factories
			//       while its fine here, the classes internally should account for the fail cases in a clean way because it is hard to track what happens otherwise

			// TODO: Testing only: truncating the table and resetting id auto-increment - remove this in production
			using (IDbContext dbContext = CreateDbContext()) {
				dbContext.Reset();
			}

			DbHelper = new DbHelper();
			DbQueryHelper = new DbQueryHelper(this);
			ViewModelCreator = new ViewModelCreator(this);
			CommandBuilder = new CommandBuilder(this);

			NoteListViewModel = ViewModelCreator.CreateNoteListViewModel();
			GroupListViewModel = ViewModelCreator.CreateGroupListViewModel();
			
			NoteTextViewModel = ViewModelCreator.CreateNoteTextViewModel();
			GroupContentsViewModel = ViewModelCreator.CreateGroupContentsViewModel();

			GroupTabsViewModel = ViewModelCreator.CreateGroupTabsViewModel();
			PrimeViewModel = ViewModelCreator.CreatePrimeViewModel();

			MainWindowViewModel = ViewModelCreator.CreateMainWindowViewModel();
		}

		public IDbContext CreateDbContext ()
		{
			return new DbContext();
		}
	}
}

namespace Mino.ViewModel
{
	public class ViewModelContext : IViewModelContext
	{
		public StateViewModel StateViewModel { get; private set; }
		public NoteListViewModel NoteListViewModel { get; private set; }
		public GroupListViewModel GroupListViewModel { get; private set; }
		public NoteTextViewModel NoteTextViewModel { get; private set; }
		public GroupContentsViewModel GroupContentsViewModel { get; private set; }
		public GroupTabsViewModel GroupTabsViewModel { get; private set; }
		public StatusBarViewModel StatusBarViewModel { get; private set; }
		public PrimeViewModel PrimeViewModel { get; private set; }
		public MainWindowViewModel MainWindowViewModel { get; private set; }

		public bool IsLoaded { get; private set; }

		public ViewModelContext (IViewModelCreator viewModelCreator)
		{
			IsLoaded = false;

			StateViewModel = viewModelCreator.CreateStateViewModel();

			NoteListViewModel = viewModelCreator.CreateNoteListViewModel();
			GroupListViewModel = viewModelCreator.CreateGroupListViewModel();

			NoteTextViewModel = viewModelCreator.CreateNoteTextViewModel(StateViewModel);
			GroupContentsViewModel = viewModelCreator.CreateGroupContentsViewModel(NoteListViewModel);

			StatusBarViewModel = viewModelCreator.CreateStatusBarViewModel();

			GroupTabsViewModel = viewModelCreator.CreateGroupTabsViewModel(
				StateViewModel, GroupListViewModel, GroupContentsViewModel);

			PrimeViewModel = viewModelCreator.CreatePrimeViewModel(
				StateViewModel, StatusBarViewModel, NoteTextViewModel, GroupTabsViewModel, NoteListViewModel);

			MainWindowViewModel = viewModelCreator.CreateMainWindowViewModel();
		}

		public void Load ()
		{
			StateViewModel.Load();
			NoteListViewModel.Load();
			GroupListViewModel.Load();
			GroupContentsViewModel.Load();
			GroupTabsViewModel.Load();
			PrimeViewModel.Load();
			NoteTextViewModel.Load();

			IsLoaded = true;
		}

		public void Shutdown ()
		{
			PrimeViewModel.Shutdown();

			IsLoaded = false;
		}
	}
}

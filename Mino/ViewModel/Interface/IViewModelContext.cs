namespace Mino.ViewModel
{
	public interface IViewModelContext
	{
		public StateViewModel StateViewModel { get; }

		public NoteListViewModel NoteListViewModel { get; }
		public GroupListViewModel GroupListViewModel { get; }

		public NoteTextViewModel NoteTextViewModel { get; }
		public GroupContentsViewModel GroupContentsViewModel { get; }

		public GroupTabsViewModel GroupTabsViewModel { get; }

		public StatusBarViewModel StatusBarViewModel { get; }

		public PrimeViewModel PrimeViewModel { get; }

		public MainWindowViewModel MainWindowViewModel { get; }

		//public void Load ();

		public void Shutdown ();
	}
}

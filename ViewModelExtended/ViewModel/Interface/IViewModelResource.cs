using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// contains resources for building / managing / controlling ViewModels
	/// </summary>
	public interface IViewModelResource
	{
		public IDbListHelper DbListHelper { get; }
		public IDbQueryHelper DbQueryHelper { get; }
		public IViewModelCreator ViewModelCreator { get; }
		public ICommandBuilder CommandBuilder { get; }

		public NoteListViewModel NoteListViewModel { get; }
		public GroupListViewModel GroupListViewModel { get; }

		public NoteTextViewModel NoteTextViewModel { get; }
		public GroupContentsViewModel GroupContentsViewModel { get; }

		public GroupTabsViewModel GroupTabsViewModel { get; }

		public StatusBarViewModel StatusBarViewModel { get; }

		public PrimeViewModel PrimeViewModel { get; }

		public MainWindowViewModel MainWindowViewModel { get; }

		public IDbContext CreateDbContext ();
	}
}

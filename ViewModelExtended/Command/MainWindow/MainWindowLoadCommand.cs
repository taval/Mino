using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class MainWindowLoadCommand : CommandBase
	{
		private readonly MainWindowViewModel f_MainWindowViewModel;

		public MainWindowLoadCommand (MainWindowViewModel mainWindowViewModel)
		{
			f_MainWindowViewModel = mainWindowViewModel;
		}

		public override void Execute (object parameter)
		{
			ViewModelContext context = (ViewModelContext)parameter;

			context.NoteListViewModel.Load();
			context.GroupListViewModel.Load();
			context.GroupContentsViewModel.Load();
			context.GroupTabsViewModel.Load();
			context.PrimeViewModel.Load();
		}
	}
}

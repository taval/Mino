using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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

			context.StateViewModel.Load();
			context.NoteListViewModel.Load();
			context.GroupListViewModel.Load();
			context.GroupContentsViewModel.Load();
			context.GroupTabsViewModel.Load();
			context.PrimeViewModel.Load();

			if (context.NoteTextViewModel.LoadCommand.CanExecute(null)) {
				context.NoteTextViewModel.LoadCommand.Execute(null);
			}
		}
	}
}

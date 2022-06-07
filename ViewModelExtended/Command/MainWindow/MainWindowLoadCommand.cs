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
			// get event args
			if (!(parameter is RoutedEventArgs)) return;

			RoutedEventArgs e = (RoutedEventArgs)parameter;

			// get window
			if (!(e.Source is MainWindow)) return;

			MainWindow window = (MainWindow)e.Source;

			// get DataContext
			if (!(window.DataContext is ViewModelContext)) return;

			ViewModelContext context = (ViewModelContext)window.DataContext;

			LoadNoteList(context, parameter);
			LoadGroupList(context, parameter);
			LoadGroupContents(context, parameter);
			LoadNoteText(context, parameter);
			LoadGroupTabs(context, parameter);
			LoadPrime(context, parameter);

			e.Handled = true;
		}

		private void LoadNoteList (ViewModelContext context, object parameter)
		{
			context.NoteListViewModel.Load();
			context.NoteListViewModel.DefaultText = UIHelper.CreateEmptyFlowDocument();
		}

		private void LoadGroupList (ViewModelContext context, object parameter)
		{
			context.GroupListViewModel.Load();
		}

		private void LoadGroupContents (ViewModelContext context, object parameter)
		{
			context.GroupContentsViewModel.Load();
		}

		private void LoadNoteText (ViewModelContext context, object parameter)
		{
			// nothing to do
		}

		private void LoadGroupTabs (ViewModelContext context, object parameter)
		{
			// NOTE: NOT creating a new group if none exist - must be done manually by design
			if (context.GroupListViewModel.Items.Count() == 0) return;

			// select the first group
			context.GroupListViewModel.Highlighted = context.GroupListViewModel.Items.First();
			GroupListObjectViewModel? highlightedGroup = context.GroupListViewModel.Highlighted;
			ICommand groupSelectCommand = context.GroupTabsViewModel.GroupSelectCommand;

			if (highlightedGroup != null) {
				if (groupSelectCommand.CanExecute(highlightedGroup)) {
					groupSelectCommand.Execute(highlightedGroup);
				}
			}

			// if no notes exist in the group, do nothing
			if (context.GroupContentsViewModel.Items.Count() == 0) return;

			// select the first note in the selected group
			context.GroupContentsViewModel.Highlighted = context.GroupContentsViewModel.Items.First();
			GroupObjectViewModel highlightedGroupNote = context.GroupContentsViewModel.Highlighted;
			ICommand groupNoteSelectCommand = context.GroupTabsViewModel.GroupNoteSelectCommand;

			if (highlightedGroupNote != null) {
				if (groupNoteSelectCommand.CanExecute(highlightedGroupNote)) {
					groupNoteSelectCommand.Execute(highlightedGroupNote);
				}
			}
		}

		private void LoadPrime (ViewModelContext context, object parameter)
		{
			// if no notes exist, create one
			if (context.NoteListViewModel.Items.Count() == 0) {
				context.PrimeViewModel.AddNote(context.NoteListViewModel.Create());
			}

			// select the first note
			context.NoteListViewModel.Highlighted = context.NoteListViewModel.Items.First();
			NoteListObjectViewModel highlighted = context.NoteListViewModel.Highlighted;
			ICommand selectCommand = context.PrimeViewModel.NoteSelectCommand;

			if (highlighted != null) {
				if (selectCommand.CanExecute(highlighted)) {
					selectCommand.Execute(highlighted);
				}
			}
		}
	}
}

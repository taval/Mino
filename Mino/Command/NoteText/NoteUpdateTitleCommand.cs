using System;
using System.Text;
using System.Windows;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteUpdateTitleCommand : CommandBase
	{
		private readonly NoteTextViewModel f_NoteTextViewModel;

		public NoteUpdateTitleCommand (NoteTextViewModel noteTextViewModel)
		{
			f_NoteTextViewModel = noteTextViewModel;
		}

		public override bool CanExecute (object parameter)
		{
			return ((ViewModelContext)Application.Current.MainWindow.DataContext).IsLoaded;
		}

		public override void Execute (object parameter)
		{
			f_NoteTextViewModel.UpdateTitle();
		}
	}
}

using System;
using System.Text;
using System.Windows;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteChangeGroupsCommand : CommandBase
	{
		private readonly PrimeViewModel f_Context;

		public NoteChangeGroupsCommand (PrimeViewModel context)
		{
			f_Context = context;
		}

		public override bool CanExecute (object parameter)
		{
			return ((ViewModelContext)Application.Current.MainWindow.DataContext).IsLoaded;
		}

		public override void Execute (object parameter)
		{
			f_Context.SetGroupsOnNote((NoteListObjectViewModel)parameter);
		}
	}
}

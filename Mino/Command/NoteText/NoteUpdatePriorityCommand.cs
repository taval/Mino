using System.Windows;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteUpdatePriorityCommand : CommandBase
	{
		private readonly NoteTextViewModel f_Context;

		public NoteUpdatePriorityCommand (NoteTextViewModel context)
		{
			f_Context = context;
		}

		public override bool CanExecute (object parameter)
		{
			return ((ViewModelContext)Application.Current.MainWindow.DataContext).IsLoaded;
		}

		public override void Execute (object parameter)
		{
			f_Context.UpdatePriority();
		}
	}
}

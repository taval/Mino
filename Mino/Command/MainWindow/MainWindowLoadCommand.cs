using Mino.ViewModel;



namespace Mino.Command
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

			context.Load();

		}
	}
}

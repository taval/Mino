using System.Windows.Input;



namespace Mino.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		#region Kit

		//private IViewModelKit f_ViewModelKit;

		#endregion



		#region Command

		public ICommand LoadCommand {
			get { return f_LoadCommand ?? throw new MissingCommandException(); }
			set { if (f_LoadCommand == null) f_LoadCommand = value; }
		}

		private ICommand? f_LoadCommand;

		public ICommand CloseCommand {
			get { return f_CloseCommand ?? throw new MissingCommandException(); }
			set { if (f_CloseCommand == null) f_CloseCommand = value; }
		}

		private ICommand? f_CloseCommand;

		#endregion



		#region Constructor

		//public MainWindowViewModel (IViewModelKit viewModelKit)
		public MainWindowViewModel ()
		{
			//f_ViewModelKit = viewModelKit;
			//f_ViewModelKit.CommandBuilder.MakeMainWindow(this);
		}

		#endregion
	}
}

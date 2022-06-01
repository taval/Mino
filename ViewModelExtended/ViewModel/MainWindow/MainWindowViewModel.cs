using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



namespace ViewModelExtended.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		#region Kit

		//private IViewModelKit f_ViewModelKit;

		#endregion



		#region Command

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

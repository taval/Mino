using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class MainWindowCloseCommand : CommandBase
	{
		private readonly MainWindowViewModel m_MainWindowViewModel;

		public MainWindowCloseCommand (MainWindowViewModel mainWindowViewModel)
		{
			m_MainWindowViewModel = mainWindowViewModel;
		}

		public override void Execute (object parameter)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}

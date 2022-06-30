using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	public class MainWindowCloseCommand : CommandBase
	{
		private readonly MainWindowViewModel f_MainWindowViewModel;

		public MainWindowCloseCommand (MainWindowViewModel mainWindowViewModel)
		{
			f_MainWindowViewModel = mainWindowViewModel;
		}

		public override void Execute (object parameter)
		{
			// move focus away from any potential event-triggering input form to perform last save before close
			IInputElement? focusedElement = FocusManager.GetFocusedElement(Application.Current.MainWindow);
			if (focusedElement != null) {
				FocusManager.SetFocusedElement(Application.Current.MainWindow, Application.Current.MainWindow);
				Keyboard.Focus(Application.Current.MainWindow);
			}

			((ViewModelContext)parameter).Shutdown();
			System.Windows.Application.Current.Shutdown();
		}
	}
}

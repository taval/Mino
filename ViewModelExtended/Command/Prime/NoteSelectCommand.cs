using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using ViewModelExtended.ViewModel;

// TODO: the way the load commands are organized, created as separate classes is not great for how Handled will be... handled.
//       Only the main window Load should have a dedicated class. Each sub-load should just be a method on MainWindowLoad. the Execute command sets the Handled value after all subfunctions are run.

namespace ViewModelExtended.Command
{
	public class NoteSelectCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteSelectCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		//public override void Execute (object parameter)
		//{
		//	//f_PrimeViewModel.SelectNote((NoteListObjectViewModel)parameter);
		//	NoteListObjectViewModel? highlighted = f_PrimeViewModel.NoteListViewModel.Highlighted;
		//	if (highlighted == null) return;
		//	f_PrimeViewModel.SelectNote(highlighted);

		//	// deserialize the Text string into a FlowDocument and set the RichTextBox's document to it
		//	if (!(parameter is MouseButtonEventArgs)) return;
		//	MouseButtonEventArgs e = (MouseButtonEventArgs)parameter;
		//	if (!(e.Source is ListViewItem)) return;
		//	ListViewItem item = (ListViewItem)e.Source;

		//	PrimeView? primeView = UIHelper.FindParent<PrimeView>(item);
		//	if (primeView == null) return;
		//	RichTextBox rtb = UIHelper.GetChildOfType<RichTextBox>(primeView);
		//	if (rtb == null) return;

		//	if (!string.IsNullOrEmpty(highlighted.Text)) {
		//		rtb.Document = (FlowDocument)XamlReader.Parse(highlighted.Text);
		//	}
		//}

		public override void Execute (object parameter)
		{
			//RoutedEventArgs e = (RoutedEventArgs)parameter;

			//if (parameter is MouseButtonEventArgs) e.Handled = true;

			// use the highlighted object as the one to select
			//NoteListObjectViewModel? highlighted = f_PrimeViewModel.NoteListViewModel.Highlighted;
			//if (highlighted == null) return;
			//f_PrimeViewModel.SelectNote(highlighted);
			NoteListObjectViewModel highlighted = (NoteListObjectViewModel)parameter;
			f_PrimeViewModel.SelectNote(highlighted);

			// find the RichTextBox
			RichTextBox rtb = UIHelper.GetChildOfType<RichTextBox>(Application.Current.MainWindow);
			if (rtb == null) return;

			// deserialize the Text string into a FlowDocument and set the RichTextBox's document to it
			if (!string.IsNullOrEmpty(highlighted.Text)) {
				rtb.Document = (FlowDocument)XamlReader.Parse(highlighted.Text);
			}
		}
	}
}

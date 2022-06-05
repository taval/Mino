using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteSelectCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteSelectCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			//f_PrimeViewModel.SelectNote((NoteListObjectViewModel)parameter);
			NoteListObjectViewModel? highlighted = f_PrimeViewModel.NoteListViewModel.Highlighted;
			if (highlighted == null) return;
			f_PrimeViewModel.SelectNote(highlighted);

			// deserialize the Text string into a FlowDocument and set the RichTextBox's document to it
			if (!(parameter is MouseButtonEventArgs)) return;
			MouseButtonEventArgs e = (MouseButtonEventArgs)parameter;
			if (!(e.Source is ListViewItem)) return;
			ListViewItem item = (ListViewItem)e.Source;

			PrimeView? primeView = UIHelper.FindParent<PrimeView>(item);
			if (primeView == null) return;
			RichTextBox rtb = UIHelper.GetChildOfType<RichTextBox>(primeView);
			if (rtb == null) return;

			if (!string.IsNullOrEmpty(highlighted.Text)) {
				rtb.Document = (FlowDocument)XamlReader.Parse(highlighted.Text);
			}
		}
	}
}

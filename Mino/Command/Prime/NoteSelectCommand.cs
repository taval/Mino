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
using Mino.ViewModel;



namespace Mino.Command
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
			// use the highlighted object as the one to select
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

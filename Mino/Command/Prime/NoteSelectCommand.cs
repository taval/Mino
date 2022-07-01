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

			f_PrimeViewModel.SelectedNoteViewModel = highlighted;

			// find the RichTextBox
			RichTextBox? rtb = UIHelper.FindChildOfType<RichTextBox>(Application.Current.MainWindow);
			if (rtb == null) return;

			// deserialize the Text string into a FlowDocument and set the RichTextBox's document to it
			if (!string.IsNullOrEmpty(highlighted.Text)) {
				rtb.Document = (FlowDocument)XamlReader.Parse(highlighted.Text);
			}
		}
	}
}

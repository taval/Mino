using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteSelectCommand : CommandBase
	{
		private readonly PrimeViewModel f_Context;

		public NoteSelectCommand (PrimeViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			// use the highlighted object as the one to select
			NoteListObjectViewModel? highlighted = parameter as NoteListObjectViewModel;

			f_Context.SelectedNoteViewModel = highlighted;

			// find the RichTextBox
			RichTextBox? rtb = UIHelper.FindChildOfType<RichTextBox>(Application.Current.MainWindow);
			if (rtb == null) return;

			// deserialize the Text string into a FlowDocument and set the RichTextBox's document to it
			if (highlighted != null && !string.IsNullOrEmpty(highlighted.Text)) {
				rtb.Document = (FlowDocument)XamlReader.Parse(highlighted.Text);
			}
			else {
				rtb.Document = (FlowDocument)XamlReader.Parse(f_Context.NoteListViewModel.DefaultText);
			}
		}
	}
}

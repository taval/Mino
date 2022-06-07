using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteChangeTextCommand : CommandBase
	{
		private readonly NoteTextViewModel f_NoteTextViewModel;

		public NoteChangeTextCommand (NoteTextViewModel noteTextViewModel)
		{
			f_NoteTextViewModel = noteTextViewModel;
		}

		public override void Execute (object parameter)
		{
			if (!(parameter is EventArgs)) return;

			RoutedEventArgs e = (RoutedEventArgs)parameter;

			if (!(e.Source is RichTextBox)) return;

			// get the FlowDocument from the RichTextBox
			RichTextBox rtb = (RichTextBox)e.Source;
			FlowDocument doc = rtb.Document;

			// serialize XAML
			MemoryStream stream = new MemoryStream();

			XamlWriter.Save(doc, stream);

			string xamlToString = Encoding.UTF8.GetString(stream.ToArray());


			

			//MessageBox.Show(xamlToString);

			f_NoteTextViewModel.Text = xamlToString;

			//f_NoteTextViewModel.UpdateText();
		}
	}
}

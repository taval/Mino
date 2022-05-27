using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using ViewModelExtended.ViewModel;

namespace ViewModelExtended.Command
{
	public class CalcCursorPosCommand : CommandBase
	{
		private readonly NoteTextViewModel f_NoteTextViewModel;

		public CalcCursorPosCommand (NoteTextViewModel noteTextViewModel)
		{
			f_NoteTextViewModel = noteTextViewModel;
		}

		public override void Execute (object parameter)
		{
			if (parameter == null || !(parameter is TextBox)) return;

			RichTextBox textBox = (RichTextBox)parameter;
			TextPointer ptr1 = textBox.Selection.Start.GetLineStartPosition(0);
			TextPointer ptr2 = textBox.Selection.Start;

			int columnNumber = ptr1.GetOffsetToPosition(ptr2);

			int bigNumber = int.MaxValue;
			int lineMoved = 0;
			int currentLineNumber = 0;

			textBox.Selection.Start.GetLineStartPosition(-bigNumber, out lineMoved);
			currentLineNumber = -lineMoved;

			f_NoteTextViewModel.LineNumber = currentLineNumber;
			f_NoteTextViewModel.ColumnNumber = columnNumber;
		}
	}
}

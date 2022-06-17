using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Mino.ViewModel;

namespace Mino.Command
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
			if (parameter == null || !(parameter is RoutedEventArgs)) return;
			RoutedEventArgs e = (RoutedEventArgs)parameter;

			if (!(e.Source is RichTextBox)) return;

			RichTextBox textBox = (RichTextBox)e.Source;
			TextPointer ptr1 = textBox.Selection.Start.GetLineStartPosition(0);
			TextPointer ptr2 = textBox.Selection.Start;

			int columnPos = ptr1.GetOffsetToPosition(ptr2);

			int columnNumber = (columnPos > 0) ? columnPos - 1 : columnPos;

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

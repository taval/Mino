using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Mino.ViewModel;



namespace Mino.Command
{
	public class CalcCursorPosCommand : CommandBase
	{
		private readonly NoteTextViewModel f_Context;

		public CalcCursorPosCommand (NoteTextViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			if (parameter == null) return;

			RoutedEventArgs? e = parameter as RoutedEventArgs;
			if (e == null || e.Handled) return;
			e.Handled = true;

			RichTextBox? textBox = e.Source as RichTextBox;
			if (textBox == null) return;

			TextPointer ptr1 = textBox.Selection.Start.GetLineStartPosition(0);
			TextPointer ptr2 = textBox.Selection.Start;

			int columnPos = ptr1.GetOffsetToPosition(ptr2);

			int columnIndex = (columnPos > 0) ? columnPos - 1 : columnPos;

			int MaxValue = int.MaxValue;
			int linePos = 0;
			int lineIndex = 0;

			textBox.Selection.Start.GetLineStartPosition(-MaxValue, out linePos);
			lineIndex = -linePos;

			f_Context.LineIndex = lineIndex;
			f_Context.ColumnIndex = columnIndex;
		}
	}
}

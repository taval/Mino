using System;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	/// <summary>
	/// through the TextBox control, place the cursor and populate Highlighted value with its data context
	/// </summary>
	public class GroupHighlightCommand : CommandBase
	{
		private readonly GroupListViewModel f_Context;

		public GroupHighlightCommand (GroupListViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			MouseButtonEventArgs? e = (MouseButtonEventArgs)parameter;

			if (e == null || e.Handled || !(e.Source is TextBox)) return;

			e.Handled = true;

			// set highlighted to the listitem datacontext
			TextBox textBox = (TextBox)e.Source;
			textBox.Focus();
			GroupListObjectViewModel dataContext = (GroupListObjectViewModel)textBox.DataContext;

			f_Context.Highlighted = dataContext;

			// if single-clicking, clear text edit selection
			if (e.ClickCount == 1) {
				textBox.SelectionLength = 0;
				return;
			};
			// otherwise (double-click) select all text for editing
			textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
		}
	}
}

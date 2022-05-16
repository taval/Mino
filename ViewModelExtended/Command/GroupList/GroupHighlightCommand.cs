using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	/// <summary>
	/// through the TextBox control, place the cursor and populate Highlighted value with its data context
	/// </summary>
	public class GroupHighlightCommand : CommandBase
	{
		private readonly GroupListViewModel m_GroupListViewModel;

		public GroupHighlightCommand (GroupListViewModel groupListViewModel)
		{
			m_GroupListViewModel = groupListViewModel;
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

			m_GroupListViewModel.Highlighted = dataContext;

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

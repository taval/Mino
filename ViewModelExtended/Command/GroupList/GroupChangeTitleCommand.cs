using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupChangeTitleCommand : CommandBase
	{
		private readonly GroupListViewModel m_GroupListViewModel;

		public GroupChangeTitleCommand (GroupListViewModel groupListViewModel)
		{
			m_GroupListViewModel = groupListViewModel;
		}

		public override void Execute (object parameter)
		{
			// check key type events for Enter key
			if (parameter is KeyEventArgs) {
				KeyEventArgs e = (KeyEventArgs)parameter;
				if (e.Key != Key.Return) return;
				if (!(e.Source is TextBox)) return;
				e.Handled = true;
				TextBox textBox = (TextBox)e.Source;
				textBox.Focus();
				ListViewItem? item = UIHelper.FindParent<ListViewItem>(textBox);
				if (item == null) return;
				item.Focus();
			}

			m_GroupListViewModel.UpdateTitle();
		}
	}
}

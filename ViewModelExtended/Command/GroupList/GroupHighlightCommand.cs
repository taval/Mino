using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;

namespace ViewModelExtended.Command
{
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

			if (e == null || !(e.Source is TextBox)) return;
			TextBox textBox = (TextBox)e.Source;
			GroupListObjectViewModel dataContext = (GroupListObjectViewModel)textBox.DataContext;

			m_GroupListViewModel.Highlighted = dataContext;
		}
	}
}

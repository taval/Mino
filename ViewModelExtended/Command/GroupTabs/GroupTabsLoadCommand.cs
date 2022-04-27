using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;

// TODO: remove - currently unused

namespace ViewModelExtended.Command
{
	public class GroupTabsLoadCommand : CommandBase
	{
		private readonly GroupTabsViewModel m_GroupTabsViewModel;

		public GroupTabsLoadCommand (GroupTabsViewModel groupTabsViewModel)
		{
			m_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			m_GroupTabsViewModel.Load();
		}
	}
}

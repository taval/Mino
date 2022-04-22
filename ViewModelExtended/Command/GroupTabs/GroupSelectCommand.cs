using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupSelectCommand : CommandBase
	{
		private readonly GroupTabsViewModel m_GroupTabsViewModel;

		public GroupSelectCommand (GroupTabsViewModel groupTabsViewModel)
		{
			m_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			m_GroupTabsViewModel.SelectGroup((GroupListObjectViewModel)parameter);
		}
	}
}

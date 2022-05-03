using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupChangeColorCommand : CommandBase
	{
		private readonly GroupListViewModel m_GroupListViewModel;

		public GroupChangeColorCommand (GroupListViewModel groupListViewModel)
		{
			m_GroupListViewModel = groupListViewModel;
		}

		public override void Execute (object parameter)
		{
			m_GroupListViewModel.UpdateColor();
		}
	}
}

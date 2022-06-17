using System;
using System.Collections.Generic;
using System.Text;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupChangeColorCommand : CommandBase
	{
		private readonly GroupListViewModel f_GroupListViewModel;

		public GroupChangeColorCommand (GroupListViewModel groupListViewModel)
		{
			f_GroupListViewModel = groupListViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupListViewModel.UpdateColor();
		}
	}
}

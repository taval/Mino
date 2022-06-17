using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupDestroyCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupDestroyCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupTabsViewModel.DestroyGroup((GroupListObjectViewModel)parameter);
		}
	}
}

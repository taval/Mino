using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteDestroyCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupNoteDestroyCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupTabsViewModel.DestroyGroupNote((GroupObjectViewModel)parameter);
		}
	}
}

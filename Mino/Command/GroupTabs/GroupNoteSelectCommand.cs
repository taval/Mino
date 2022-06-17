using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupNoteSelectCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupNoteSelectCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupTabsViewModel.SelectGroupNote((GroupObjectViewModel)parameter);
		}
	}
}

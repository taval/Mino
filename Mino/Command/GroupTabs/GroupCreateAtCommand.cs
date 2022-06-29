using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupCreateAtCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupCreateAtCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			// focus on the GroupList tab
			TabControl? tabControl =
				(TabControl?)UIHelper.FindChildOrNull<TabControl>(Application.Current.MainWindow, "GroupTabControl");

			if (tabControl == null) {
				return;
			}
			TabItem? tabItem = (TabItem)tabControl.FindName("GroupListTab");
			if (tabItem == null) {
				return;
			}
			tabControl.SelectedItem = tabItem;

			// add a new Group to the GroupList
			f_GroupTabsViewModel.CreateGroupAt((GroupListObjectViewModel)parameter);
		}
	}
}

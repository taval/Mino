using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupCreateCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupCreateCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			// focus on the GroupList tab
			TabControl? tabControl = UIHelper.FindChild<TabControl>(Application.Current.MainWindow, "GroupTabControl");
			if (tabControl == null) {
				return;
			}
			TabItem? tabItem = (TabItem)tabControl.FindName("GroupListTab");
			if (tabItem == null) {
				return;
			}
			tabControl.SelectedItem = tabItem;

			// add a new Group to the GroupList
			f_GroupTabsViewModel.CreateGroup(
				f_GroupTabsViewModel.Resource.GroupListViewModel.Highlighted,
				f_GroupTabsViewModel.Resource.GroupListViewModel.Create()
			);
		}
	}
}

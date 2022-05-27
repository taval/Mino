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
	public class GroupSelectCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupSelectCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupTabsViewModel.SelectGroup((GroupListObjectViewModel)parameter);

			// focus on the GroupList tab
			TabControl? tabControl = UIHelper.FindChild<TabControl>(Application.Current.MainWindow, "GroupTabControl");
			if (tabControl == null) {
				return;
			}
			TabItem? tabItem = (TabItem)tabControl.FindName("GroupContentsTab");
			if (tabItem == null) {
				return;
			}
			tabControl.SelectedItem = tabItem;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino.Command
{
	public class SwitchTabsCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public SwitchTabsCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			TabControl? tabControl = UIHelper.FindChild<TabControl>(Application.Current.MainWindow, "GroupTabControl");
			if (tabControl == null) return;
			TabItem? tabItem = (TabItem)tabControl.FindName("GroupContentsTab");
			if (tabItem == null) return;

			if ((f_GroupTabsViewModel.IsGroupSelected == false) || (f_GroupTabsViewModel.HasGroup == false)) {
				// prevent tab switch
				if (tabControl.SelectedItem == tabItem) {
					tabControl.SelectedIndex = f_GroupTabsViewModel.SelectedTabIndex;
				}
			}
			f_GroupTabsViewModel.SelectedTabIndex = tabControl.SelectedIndex;
		}
	}
}

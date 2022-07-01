using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
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
			// get event args
			MouseEventArgs e = (MouseEventArgs)parameter;

			if (e == null ||
				e.Handled ||
				e.LeftButton != MouseButtonState.Pressed ||
				!(e.Source is FrameworkElement)) return;

			e.Handled = true;

			FrameworkElement source = (FrameworkElement)e.Source;

			ListViewItem? item = source as ListViewItem;
			if (item == null) return;

			// prevent selection if over the remove button
			Button? closeButton = (Button?)UIHelper.FindChildOrNull<Button>(item, "RemoveItemButton");

			if (closeButton?.IsMouseOver == true) return;

			if (!(item.DataContext is GroupListObjectViewModel)) return;

			GroupListObjectViewModel viewModel = (GroupListObjectViewModel)item.DataContext;

			if (!GroupTitleRule.IsValidGroupTitle(viewModel.Title)) return;

			// perform selection
			f_GroupTabsViewModel.SelectedGroupViewModel = viewModel;

			// focus on the GroupList tab
			TabControl? tabControl = UIHelper.FindChildOfType<TabControl>(Application.Current.MainWindow);

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

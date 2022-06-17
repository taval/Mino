using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupPickupCommand : CommandBase
	{
		private readonly GroupListViewModel f_ListViewModel;

		public GroupPickupCommand (GroupListViewModel listViewModel)
		{
			f_ListViewModel = listViewModel;
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

			// if source was text box, bail
			if (e.OriginalSource is TextBox) return;

			// if button not pressed, invalid event source, or close button is under mouse, bail out
			Button? closeButton = UIHelper.FindChild<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");
			if (closeButton?.IsMouseOver == true) return;

			// get event source
			FrameworkElement source = (FrameworkElement)e.Source;

			// get ListView name
			ListViewItem? item = source.TemplatedParent as ListViewItem;
			if (item == null) return;
			ListView listView = (ListView)ItemsControl.ItemsControlFromItemContainer(item);
			string listName = listView.Name;

			// get data to pick up
			object? dataContext = source.DataContext;
			if (dataContext == null) return;

			// pick up data
			DragDropEffects _ = DragDrop.DoDragDrop(
				item,
				new DataObject(
					DataFormats.Serializable,
					new Tuple<string, object>(listName, dataContext)),
				DragDropEffects.Copy);
		}
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupNotePickupCommand : CommandBase
	{
		private readonly GroupContentsViewModel f_ListViewModel;

		public GroupNotePickupCommand (GroupContentsViewModel listViewModel)
		{
			f_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			MouseEventArgs e = (MouseEventArgs)parameter;

			// if button not pressed, invalid event source, or close button is under mouse, bail out
			Button? closeButton =
				(Button?)UIHelper.FindChildOrNull<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (e == null ||
				e.Handled ||
				e.LeftButton != MouseButtonState.Pressed ||
				!(e.Source is FrameworkElement) ||
				closeButton?.IsMouseOver == true) return;

			e.Handled = true;

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

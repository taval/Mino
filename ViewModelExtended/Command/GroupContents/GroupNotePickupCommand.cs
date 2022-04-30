using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNotePickupCommand : CommandBase
	{
		private readonly GroupContentsViewModel m_ListViewModel;

		public GroupNotePickupCommand (GroupContentsViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			MouseEventArgs e = (MouseEventArgs)parameter;

			// if button not pressed, invalid event source, or close button is under mouse, bail out
			Button? closeButton = UIHelper.FindChild<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (e == null ||
				e.LeftButton != MouseButtonState.Pressed ||
				!(e.Source is FrameworkElement) ||
				closeButton?.IsMouseOver == true) {
				return;
			}

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
				DragDropEffects.Link);
		}
	}
}

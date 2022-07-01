using System;
using System.Windows;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteReorderCommand : CommandBase
	{
		private readonly NoteListViewModel f_ListViewModel;

		public NoteReorderCommand (NoteListViewModel listViewModel)
		{
			f_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			DragEventArgs e = (DragEventArgs)parameter;

			// prevent close button from performing operation
			Button? closeButton =
				(Button?)UIHelper.FindChildOrNull<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (e == null || e.Handled || !(e.Source is FrameworkElement) || closeButton?.IsMouseOver == true) return;

			e.Handled = true;

			// get the event source element
			FrameworkElement element = (FrameworkElement)e.Source;

			// get the data from DragDrop operation
			Tuple<string, object> data = (Tuple<string, object>)e.Data.GetData(DataFormats.Serializable);

			// if the item comes from different ListView, bail out
			string itemListName = data.Item1;

			// get listview
			ListViewItem? item = element.TemplatedParent as ListViewItem;
			if (item == null) return;
			ListView listView = (ListView)ItemsControl.ItemsControlFromItemContainer(item);

			if (!itemListName.Equals(listView.Name)) return;

			// do scroll
			UIHelper.ScrollListView(e, listView);

			// get target
			NoteListObjectViewModel? target = element.DataContext as NoteListObjectViewModel;

			// check source for correct type
			if (target == null || data.Item2 == null || !(data.Item2 is NoteListObjectViewModel)) return;

			// get source
			NoteListObjectViewModel source = (NoteListObjectViewModel)data.Item2;

			// reorder
			f_ListViewModel.Reorder(source, target);
		}
	}
}

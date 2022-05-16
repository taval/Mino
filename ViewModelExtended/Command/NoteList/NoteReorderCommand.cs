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
	public class NoteReorderCommand : CommandBase
	{
		private readonly NoteListViewModel m_ListViewModel;

		public NoteReorderCommand (NoteListViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			DragEventArgs e = (DragEventArgs)parameter;

			// prevent close button from performing operation
			Button? closeButton = UIHelper.FindChild<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");
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

			if (!itemListName.Equals(listView.Name)) return; // TODO: copy this version of the name comparison to the other reorder commands to separate them from a particular view instance

			// do scroll
			//ScrollListView(e, listView); // TODO: uncomment and test when dragleave is stable, then incorporate into the other listviews

			// get target
			NoteListObjectViewModel? target = element.DataContext as NoteListObjectViewModel;

			// check source for correct type
			if (target == null || data.Item2 == null || !(data.Item2 is NoteListObjectViewModel)) return;

			// get source
			NoteListObjectViewModel source = (NoteListObjectViewModel)data.Item2;

			// reorder
			m_ListViewModel.Reorder(source, target);
		}

		private void ScrollListView (DragEventArgs e, ListView listView)
		{
			ScrollViewer? scrollViewer = UIHelper.GetChildOfType<ScrollViewer>(listView);
			
			if (scrollViewer != null) {
				double tolerance = 60;
				double verticalPos = e.GetPosition(listView).Y;
				double offset = 1;

				if (verticalPos < tolerance) { // if top of visible list, scroll up
					scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
				}
				else if (verticalPos > listView.ActualHeight - tolerance) { // if bottom of visible list, scroll down
					scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
				}
			}
		}
	}
}

//FrameworkElement container = sender as FrameworkElement;

//if (container == null) { return; }

//ScrollViewer scrollViewer = GetFirstVisualChild<ScrollViewer>(container);

//if (scrollViewer == null) { return; }

//double tolerance = 60;
//double verticalPos = e.GetPosition(container).Y;
//double offset = 20;

//if (verticalPos < tolerance) // Top of visible list? 
//{
//	//Scroll up
//	scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
//}
//else if (verticalPos > container.ActualHeight - tolerance) //Bottom of visible list? 
//{
//	//Scroll down
//	scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
//}
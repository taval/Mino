using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NotePreselectCommand : CommandBase
	{
		private readonly NoteListViewModel m_ListViewModel;

		public NotePreselectCommand (NoteListViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			MouseButtonEventArgs? e = parameter as MouseButtonEventArgs;
			Button? closeButton = UIHelper.FindChild<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (e == null ||
				e.LeftButton != MouseButtonState.Pressed ||
				e.ClickCount != 1 ||
				!(e.Source is FrameworkElement) ||
				closeButton?.IsMouseOver == true) {
				return;
			}

			FrameworkElement? source = e.Source as FrameworkElement;

			if (source == null) {
				return;
			}

			object dataContext = source.DataContext;

			ListViewItem? element = source.TemplatedParent as ListViewItem;

			if (element == null) {
				return;
			}

			// set the highlighted item
			m_ListViewModel.Highlighted = dataContext as NoteListObjectViewModel;

			// capture item for drag drop operation
			DragDropEffects dragDropResult = DragDrop.DoDragDrop(
				element, new DataObject(DataFormats.Serializable, dataContext), DragDropEffects.Link);

		}
	}
}





/// <summary>
/// 1) if the mouse moves within an item with mouse pressed, capture and give data to DragDrop
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected override void Item_MouseMove (object sender, MouseEventArgs e)
//{
//	// FrameworkElement derives from DependencyObject
//	if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement frameworkElement) {
//		object note = frameworkElement.DataContext; // preserve the state prior to removal in case of cancel

//		DragDropEffects dragDropResult = DragDrop.DoDragDrop(
//			frameworkElement, new DataObject(DataFormats.Serializable, note), DragDropEffects.Link);
//	}

//}
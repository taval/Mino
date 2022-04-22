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
	public class GroupNotePreselectCommand : CommandBase
	{
		private readonly GroupContentsViewModel m_ListViewModel;

		public GroupNotePreselectCommand (GroupContentsViewModel listViewModel)
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
			m_ListViewModel.Highlighted = dataContext as GroupObjectViewModel;

			DataObject data = new DataObject();
			data.SetData("Object", m_ListViewModel);
			data.SetData(DataFormats.Serializable, dataContext);

			// capture item for drag drop operation
			DragDropEffects dragDropResult = DragDrop.DoDragDrop(
				element, data, DragDropEffects.Link);

		}
	}
}

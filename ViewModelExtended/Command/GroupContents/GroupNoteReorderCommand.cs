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
	public class GroupNoteReorderCommand : CommandBase
	{
		private readonly GroupContentsViewModel m_ListViewModel;

		public GroupNoteReorderCommand (GroupContentsViewModel listViewModel)
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

			// if the item comes from the same ListView, bail out
			string itemListName = data.Item1;
			if (!itemListName.Equals("ListView_GroupContentsView")) return;

			// get target
			GroupObjectViewModel? target = element.DataContext as GroupObjectViewModel;
			
			// check source for correct type
			if (target == null || data.Item2 == null || !(data.Item2 is GroupObjectViewModel)) return;

			// get source
			GroupObjectViewModel source = (GroupObjectViewModel)data.Item2;

			// reorder
			m_ListViewModel.Reorder(source, target);
		}
	}
}

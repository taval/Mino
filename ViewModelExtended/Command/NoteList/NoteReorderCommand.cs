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
			if (!(e.Source is FrameworkElement) || closeButton?.IsMouseOver == true) return;

			// get the event source element
			FrameworkElement element = (FrameworkElement)e.Source;

			// get the data from DragDrop operation
			Tuple<string, object> data = (Tuple<string, object>)e.Data.GetData(DataFormats.Serializable);

			// if the item comes from the same ListView, bail out
			string itemListName = data.Item1;
			if (!itemListName.Equals("ListView_NoteListView")) return;

			// get target
			NoteListObjectViewModel? target = element.DataContext as NoteListObjectViewModel;

			// check source for correct type
			if (target == null || data.Item2 == null || !(data.Item2 is NoteListObjectViewModel)) return;

			// get source
			NoteListObjectViewModel source = (NoteListObjectViewModel)data.Item2;

			// reorder
			m_ListViewModel.Reorder(source, target);

			// refresh list data
			//m_ListViewModel.RefreshListView();
		}
	}
}

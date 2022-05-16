using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ViewModelExtended.ViewModel;

// TODO: write over duplicates in modified queue instead of accumulating many duplicates

namespace ViewModelExtended.Command
{
	public class NoteDropCommand : CommandBase
	{
		private readonly NoteListViewModel m_ListViewModel;

		public NoteDropCommand (NoteListViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			DragEventArgs e = (DragEventArgs)parameter;

			if (e == null || e.Handled || !(e.Source is FrameworkElement)) return;

			e.Handled = true;

			// get the event source element
			FrameworkElement element = (FrameworkElement)e.Source;

			// get the data from DragDrop operation
			Tuple<string, object> data = (Tuple<string, object>)e.Data.GetData(DataFormats.Serializable);

			// if the item comes from different ListView, bail out
			string itemListName = data.Item1;
			if (!itemListName.Equals("ListView_NoteListView")) return;

			m_ListViewModel.ReorderCommit();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ViewModelExtended.ViewModel;

/** TODO: if cursor is outside of listview on drop, rollback (keep original state in addition to queue of changes) - see class NoteCancelDropCommand
 * UPDATE: partially working: if leaving the ListView area at the wrong angle or speed, the rollback does not occur. The lower x and y constraints only sometimes prevent this. The ListView area does not seem to sync up with the HitTest area.
 * UPDATE 2: the common factors in any of the attempted fixes are the top border sometimes failing, dependent on speed or closeness to edge. about 4 px gives a better ratio than most other values. Possible fixes may involve capturing samples of hittests or using callbacks
 */

// TODO: write over duplicates in modified queue instead of accumulating many duplicates

namespace ViewModelExtended.Command
{
	public class NoteCancelDropCommand : CommandBase
	{
		private readonly NoteListViewModel m_ListViewModel;

		public NoteCancelDropCommand (NoteListViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			// get event args
			DragEventArgs e = (DragEventArgs)parameter;

			if (e == null || e.Handled || !(e.Source is FrameworkElement)) return;

			e.Handled = true;

			FrameworkElement listView = (FrameworkElement)e.Source;

			Point pos = e.GetPosition(listView);
			HitTestResult result = VisualTreeHelper.HitTest(listView, pos);

			// if out of bounds, rollback the reordering
			if (result == null || pos.X < 4 || pos.Y < 4) {
				m_ListViewModel.CancelReorder();
			}
		}
	}
}

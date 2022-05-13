using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ViewModelExtended.ViewModel;



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
			DragEventArgs e = (DragEventArgs)parameter;
			if (e.Handled) return;
			//m_ListViewModel.CancelReorder();
			e.Handled = true;
		}
	}
}

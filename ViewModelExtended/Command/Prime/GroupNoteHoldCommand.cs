using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteHoldCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public GroupNoteHoldCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			DragEventArgs e = (DragEventArgs)parameter;
			FrameworkElement listView = (FrameworkElement)e.Source;

			HitTestResult result = VisualTreeHelper.HitTest(listView, e.GetPosition(listView));

			// if out of bounds of hit test, remove the item from the list
			if (result == null) {
				m_PrimeViewModel.HoldGroupNote();
			}
			
		}
	}
}

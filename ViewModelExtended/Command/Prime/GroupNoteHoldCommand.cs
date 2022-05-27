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
		private readonly PrimeViewModel f_PrimeViewModel;

		public GroupNoteHoldCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			DragEventArgs e = (DragEventArgs)parameter;
			if (e == null || e.Handled || !(e.Source is FrameworkElement)) return;

			e.Handled = true;

			FrameworkElement listView = (FrameworkElement)e.Source;

			HitTestResult result = VisualTreeHelper.HitTest(listView, e.GetPosition(listView));

			// if out of bounds of hit test, remove the item from the list
			if (result == null) {
				f_PrimeViewModel.HoldGroupNote();
			}
		}
	}
}

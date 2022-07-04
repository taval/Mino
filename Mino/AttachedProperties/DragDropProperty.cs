using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Mino
{
	public class DragDropProperty : DependencyObject
	{
		/// <summary>
		/// a notification that a (highlighted) item is ready for DragDrop operation
		/// </summary>
		public static readonly DependencyProperty IsDropReady = DependencyProperty.RegisterAttached(
			nameof(IsDropReady), typeof(bool), typeof(DragDropProperty), new FrameworkPropertyMetadata(
				defaultValue: false));
		public static bool GetIsDropReady (DependencyObject o) => (bool)o.GetValue(IsDropReady);
		public static void SetIsDropReady (DependencyObject o, bool val) => o.SetValue(IsDropReady, val);

		/// <summary>
		/// temporarily back out and hold onto item to be added from an external source
		/// </summary>
		public static readonly DependencyProperty HoldCommand = DependencyProperty.RegisterAttached(
			nameof(HoldCommand), typeof(ICommand), typeof(DragDropProperty), new PropertyMetadata(null));
		public static ICommand GetHoldCommand (DependencyObject o) => (ICommand)o.GetValue(HoldCommand);
		public static void SetHoldCommand (DependencyObject o, ICommand val) => o.SetValue(HoldCommand, val);

		/// <summary>
		/// commit the added item from an external source
		/// </summary>
		public static readonly DependencyProperty DropCommand = DependencyProperty.RegisterAttached(
			nameof(DropCommand), typeof(ICommand), typeof(DragDropProperty), new PropertyMetadata(null));
		public static ICommand GetDropCommand (DependencyObject o) => (ICommand)o.GetValue(DropCommand);
		public static void SetDropCommand (DependencyObject o, ICommand val) => o.SetValue(DropCommand, val);
	}
}

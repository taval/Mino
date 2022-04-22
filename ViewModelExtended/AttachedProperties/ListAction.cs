using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

/** TODO: states and actions in use after NoteList EventTrigger conversion:
 * local:ListAction.InsertCommand
 * local:ListAction.SelectCommand
 * local:ListAction.RemoveCommand
 * 
 * remove all others
 */

namespace ViewModelExtended
{
	public class ListAction : DependencyObject
	{
		/// <summary>
		/// move an item to a different position in the list
		/// </summary>
		public static readonly DependencyProperty ReorderCommand = DependencyProperty.RegisterAttached(
			nameof(ReorderCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetReorderCommand (DependencyObject o) => (ICommand)o.GetValue(ReorderCommand);
		public static void SetReorderCommand (DependencyObject o, ICommand val) => o.SetValue(ReorderCommand, val);

		/// <summary>
		/// add an item from an external source
		/// </summary>
		public static readonly DependencyProperty SendCommand = DependencyProperty.RegisterAttached(
			nameof(SendCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetSendCommand (DependencyObject o) => (ICommand)o.GetValue(SendCommand);
		public static void SetSendCommand (DependencyObject o, ICommand val) => o.SetValue(SendCommand, val);

		/// <summary>
		/// add an item from an external source
		/// </summary>
		public static readonly DependencyProperty ReceiveCommand = DependencyProperty.RegisterAttached(
			nameof(ReceiveCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetReceiveCommand (DependencyObject o) => (ICommand)o.GetValue(ReceiveCommand);
		public static void SetReceiveCommand (DependencyObject o, ICommand val) => o.SetValue(ReceiveCommand, val);

		/// <summary>
		/// remove an item from the list
		/// </summary>
		public static readonly DependencyProperty RemoveCommand = DependencyProperty.RegisterAttached(
			nameof(RemoveCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetRemoveCommand (DependencyObject o) => (ICommand)o.GetValue(RemoveCommand);
		public static void SetRemoveCommand (DependencyObject o, ICommand val) => o.SetValue(RemoveCommand, val);

		/// <summary>
		/// select an item - e.g. queue it up for consumption by TextView
		/// </summary>
		public static readonly DependencyProperty SelectCommand = DependencyProperty.RegisterAttached(
			nameof(SelectCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetSelectCommand (DependencyObject o) => (ICommand)o.GetValue(SelectCommand);
		public static void SetSelectCommand (DependencyObject o, ICommand val) => o.SetValue(SelectCommand, val);

		/// <summary>
		/// differentiate a currently chosen item without presenting it to view
		/// </summary>
		public static readonly DependencyProperty HighlightCommand = DependencyProperty.RegisterAttached(
			nameof(HighlightCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetHighlightCommand (DependencyObject o) => (ICommand)o.GetValue(HighlightCommand);
		public static void SetHighlightCommand (DependencyObject o, ICommand val) => o.SetValue(HighlightCommand, val);

		/// <summary>
		/// add a new item to the list
		/// </summary>
		public static readonly DependencyProperty InsertCommand = DependencyProperty.RegisterAttached(
			nameof(InsertCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetInsertCommand (DependencyObject o) => (ICommand)o.GetValue(InsertCommand);
		public static void SetInsertCommand (DependencyObject o, ICommand val) => o.SetValue(InsertCommand, val);

		/// <summary>
		/// resend data
		/// </summary>
		public static readonly DependencyProperty RefreshCommand = DependencyProperty.RegisterAttached(
			nameof(RefreshCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetRefreshCommand (DependencyObject o) => (ICommand)o.GetValue(RefreshCommand);
		public static void SetRefreshCommand (DependencyObject o, ICommand val) => o.SetValue(RefreshCommand, val);


		/// <summary>
		/// differentiate a currently chosen item without presenting it to view
		/// </summary>
		public static readonly DependencyProperty DummyCommand = DependencyProperty.RegisterAttached(
			nameof(DummyCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetDummyCommand (DependencyObject o) => (ICommand)o.GetValue(DummyCommand);
		public static void SetDummyCommand (DependencyObject o, ICommand val) => o.SetValue(DummyCommand, val);

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty PreselectCommand = DependencyProperty.RegisterAttached(
			nameof(PreselectCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetPreselectCommand (DependencyObject o) => (ICommand)o.GetValue(PreselectCommand);
		public static void SetPreselectCommand (DependencyObject o, ICommand val) => o.SetValue(PreselectCommand, val);

	}
}

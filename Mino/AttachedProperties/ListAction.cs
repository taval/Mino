using System.Windows;
using System.Windows.Input;



namespace Mino
{
	public class ListAction : DependencyObject
	{
		/// <summary>
		/// remove an item from the list
		/// </summary>
		public static readonly DependencyProperty RemoveCommand = DependencyProperty.RegisterAttached(
			nameof(RemoveCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetRemoveCommand (DependencyObject o) => (ICommand)o.GetValue(RemoveCommand);
		public static void SetRemoveCommand (DependencyObject o, ICommand val) => o.SetValue(RemoveCommand, val);

		/// <summary>
		/// add a new item to the list
		/// </summary>
		public static readonly DependencyProperty CreateAtCommand = DependencyProperty.RegisterAttached(
			nameof(CreateAtCommand), typeof(ICommand), typeof(ListAction), new PropertyMetadata(null));
		public static ICommand GetCreateAtCommand (DependencyObject o) => (ICommand)o.GetValue(CreateAtCommand);
		public static void SetCreateAtCommand (DependencyObject o, ICommand val) => o.SetValue(CreateAtCommand, val);
	}
}

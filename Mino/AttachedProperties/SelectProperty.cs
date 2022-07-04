using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;



namespace Mino
{
	public class SelectProperty : DependencyObject
	{
		/// <summary>
		/// select an item - e.g. queue it up for consumption by TextView
		/// </summary>
		public static readonly DependencyProperty SelectCommand = DependencyProperty.RegisterAttached(
			nameof(SelectCommand), typeof(ICommand), typeof(SelectProperty), new PropertyMetadata(null));
		public static ICommand GetSelectCommand (DependencyObject o) => (ICommand)o.GetValue(SelectCommand);
		public static void SetSelectCommand (DependencyObject o, ICommand val) => o.SetValue(SelectCommand, val);


		public static readonly DependencyProperty SelectedTitle = DependencyProperty.RegisterAttached(
		nameof(SelectedTitle), typeof(string), typeof(SelectProperty), new FrameworkPropertyMetadata(
		defaultValue: String.Empty, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static string GetSelectedTitle (DependencyObject o) => (string)o.GetValue(SelectedTitle);
		public static void SetSelectedTitle (DependencyObject o, string val) => o.SetValue(SelectedTitle, val);
	}
}

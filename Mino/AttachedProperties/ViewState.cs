using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;



namespace Mino
{
	public class ViewState : DependencyObject
	{
		public static readonly DependencyProperty ElementHeight = DependencyProperty.RegisterAttached(
			nameof(ElementHeight), typeof(int), typeof(ViewState), new FrameworkPropertyMetadata(defaultValue: 0));
		public static int GetElementHeight (DependencyObject o) => (int)o.GetValue(ElementHeight);
		public static void SetElementHeight (DependencyObject o, int val) => o.SetValue(ElementHeight, val);
	}
}

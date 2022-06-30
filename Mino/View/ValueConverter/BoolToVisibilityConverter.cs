using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;



namespace Mino
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityConverter : BaseConverter, IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value) {
				return Visibility.Visible;
			}
			return Visibility.Hidden;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("no case exists for Collapsed so there is no direct conversion to boolean");
		}
	}
}

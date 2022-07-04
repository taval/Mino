using System;
using System.Globalization;
using System.Windows.Data;



namespace Mino
{
	/// <summary>
	/// subtracts parameter from value input. clamped to positive number
	/// </summary>
	[ValueConversion(typeof(int), typeof(string))]
	public class SubtractionConverter : BaseConverter, IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			int? val = value as int?;
			int intVal = val ?? 0;

			int subtraction = Int32.Parse((string)parameter);
	
			int output = Math.Clamp(intVal - subtraction, 0, int.MaxValue);

			return output.ToString();
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

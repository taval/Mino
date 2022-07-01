using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;



namespace Mino
{
	[ValueConversion(typeof(int), typeof(string))]
	public class PriorityToColorConverter : BaseConverter, IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string output = String.Empty;
			Color priorityOne = Color.FromArgb(180, 240, 180);
			Color priorityTwo = Color.FromArgb(240, 240, 180);
			Color priorityThree = Color.FromArgb(240, 180, 180);

			switch ((int)value) {
				case 1: output = ColorTranslator.ToHtml(priorityTwo); break;
				case 2: output = ColorTranslator.ToHtml(priorityThree); break;
				default: output = ColorTranslator.ToHtml(priorityOne); break;
			}

			return output;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			int output = -1;
			Color priorityTwo = Color.FromArgb(255, 255, 0);
			Color priorityThree = Color.FromArgb(255, 0, 0);
			string input = (string)value;

			if (input.Equals(ColorTranslator.ToHtml(priorityTwo))) {
				output = 1;
			}
			else if (input.Equals(ColorTranslator.ToHtml(priorityThree))) {
				output = 2;
			}
			else {
				output = 0;
			}

			return output;
		}
	}
}

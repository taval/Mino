using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;



namespace Mino
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToStubConverter : BaseConverter, IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return String.Empty;

           string text = (string)value;

            if (text.Length >= 16) {
                return text.Substring(0, 13) + "...";
            }
            else {
                return text;
			}
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("this cannot be converted to original string from its current state");
        }
    }
}

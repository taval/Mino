using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;



namespace Mino
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class XamlStringToFlowDocumentConverter : BaseConverter, IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (FlowDocument)XamlReader.Parse(value as string);
        }

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			MemoryStream stream = new MemoryStream();

			XamlWriter.Save(value, stream);

			return Encoding.UTF8.GetString(stream.ToArray());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;



namespace Mino
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FlowDocumentStringToStubConverter : BaseConverter, IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            XamlStringToFlowDocumentConverter converter = new XamlStringToFlowDocumentConverter();
            FlowDocument flowDocument = (FlowDocument)converter.Convert(value, targetType, parameter, culture);
            Block? firstBlock = flowDocument.Blocks.FirstBlock;
            if (firstBlock == null) return string.Empty;
            Paragraph p = (Paragraph)firstBlock;
            //string text = ((Run)p.Inlines.FirstInline).Text;
            string text = String.Empty;
            foreach (Run run in p.Inlines) {
                text += run.Text;
			}

            if (text.Length >= 16) {
                return text.Substring(0, 16);
            }
            else {
                return text;
			}
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("this cannot be converted to FlowDocument from its current state");
        }
    }
}

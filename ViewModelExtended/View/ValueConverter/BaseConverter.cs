using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;



namespace ViewModelExtended
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue (IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

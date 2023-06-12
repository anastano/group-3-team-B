using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIMS_HCI_Project.Wpf.Converters
{
    public class StringToTimeOnlyConverter : MarkupExtension, IValueConverter
    {
        public StringToTimeOnlyConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (TimeOnly)value;
            
            return String.Format("{0:00}:{1:00}", v.Hour, v.Minute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as string;
            TimeOnly ret = new TimeOnly();
            if (TimeOnly.TryParse(v, out ret))
            {
                return ret;
            }
            else
            {
                return value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIMS_HCI_Project.Wpf.Converters
{
    public class StringToIntConverter : MarkupExtension, IValueConverter
    {
        public StringToIntConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (int)value;

            return v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as string;
            int ret = 0;
            if(int.TryParse(v, out ret))
            {
                return ret;
            }
            else
            {
                return 0;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIMS_HCI_Project.WPF.Converters
{
    public class AllTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // return ((ObservableCollection<int>)value) == -1 ? "All Time" : (object?)value.ToString();
            if (value is int year && year > 0)
            {
                return year.ToString();
            }
            else
            {
                return "AllTime";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

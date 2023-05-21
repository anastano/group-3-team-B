using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SIMS_HCI_Project.WPF.Converters
{
    public class GraphBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            Color color = Color.FromArgb(255, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

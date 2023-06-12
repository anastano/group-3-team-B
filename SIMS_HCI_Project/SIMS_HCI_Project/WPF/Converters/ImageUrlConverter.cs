using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SIMS_HCI_Project.WPF.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = "https://thumbs.dreamstime.com/z/no-image-available-icon-photo-camera-flat-vector-illustration-132483141.jpg";
            List<string> urls = (List<string>)value;
            if (urls.Count > 0 && urls[0] != "")
            {
                url = urls[0];      
            }
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(url, UriKind.Absolute);
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

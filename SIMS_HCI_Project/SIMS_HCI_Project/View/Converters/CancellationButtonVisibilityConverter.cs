using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIMS_HCI_Project.View.Converters
{
    public class CancellationButtonVisibilityConverter : MarkupExtension, IMultiValueConverter
    {
        public CancellationButtonVisibilityConverter() { }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int deadline = (int)values[0];
            DateTime startTime = (DateTime)values[1];
            bool isVisible = false;
            if(startTime <= DateTime.Today)
            {
                return ButtonVisibility(isVisible);
            }
            isVisible = ((startTime - DateTime.Today).TotalDays - 1) >= deadline;
            return ButtonVisibility(isVisible);
        }
        public Visibility ButtonVisibility(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

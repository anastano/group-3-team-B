using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Linq;

namespace SIMS_HCI_Project.WPF.Converters
{
    internal class ForumUsefulConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<ForumComment> comments = (List<ForumComment>)value;
            bool ownerCommentsFlag = comments.FindAll(c => c.IsUseful == true && c.User.Role == UserRole.OWNER).Count >= 10;
            bool guestCommentsFlag = comments.FindAll(c => c.IsUseful == true && (c.User.Role == UserRole.GUEST1 || c.User.Role == UserRole.GUEST2)).Count >= 20;
            return (ownerCommentsFlag && guestCommentsFlag);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

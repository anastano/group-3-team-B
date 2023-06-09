using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_HCI_Project.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for LeaveCommentView.xaml
    /// </summary>
    public partial class LeaveCommentView : Window
    {
        public LeaveCommentView(Owner owner, Forum forum)
        {
            InitializeComponent();
            this.DataContext = new LeaveCommentViewModel(this, owner, forum);
        }
    }
}

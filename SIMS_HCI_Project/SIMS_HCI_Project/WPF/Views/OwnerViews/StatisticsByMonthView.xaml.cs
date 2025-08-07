using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
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
    /// Interaction logic for StatisticsByMonthView.xaml
    /// </summary>
    public partial class StatisticsByMonthView : Window
    {
        public StatisticsByMonthView(StatisticsByYearView statisticsByYearView, Accommodation accommodation, AccommodationYear year)
        {
            InitializeComponent();
            this.DataContext = new StatisticsByMonthViewModel(this, statisticsByYearView, accommodation, year);
        }
    }
}

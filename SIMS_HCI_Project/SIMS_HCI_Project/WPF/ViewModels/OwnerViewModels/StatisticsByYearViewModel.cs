using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByYearViewModel
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationStatisticsService _statisticsService;

        public StatisticsByYearView StatisticsByYearView { get; set; }
        public Accommodation Accommodation { get; set; }
        public int BestYear { get; set; }
        public List<AccommodationYear> AccommodationYears { get; set; }
        public AccommodationYear SelectedYear { get; set; }

        public RelayCommand CloseMonthStatisticsViewCommand { get; set; }
        public RelayCommand SelectYearForStatisticsCommand { get; set; }

        public StatisticsByYearViewModel(StatisticsByYearView statisticsByYearView, 
            AccommodationReservationService reservationService, AccommodationStatisticsService statisticsService, 
            Accommodation selectedAccommodation) 
        {
            InitCommands();

            _reservationService = reservationService;
            _statisticsService = statisticsService;

            StatisticsByYearView = statisticsByYearView;

            Accommodation = selectedAccommodation;
            BestYear = _statisticsService.FindBestYear(Accommodation.Id);
            AccommodationYears = _statisticsService.GetYearsByAccommodationId(Accommodation.Id);
        }

        #region Commands
        public void Executed_SelectYearForStatisticsCommand(object obj)
        {
            if (SelectedYear != null)
            {
                Window statisticsByMonth = new StatisticsByMonthView(StatisticsByYearView,
                _reservationService, _statisticsService, Accommodation, SelectedYear);
                statisticsByMonth.ShowDialog();
            }
            else
            {
                MessageBox.Show("No year has been selected");
            }
        }

        public bool CanExecute_SelectYearForStatisticsCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseMonthStatisticsViewCommand(object obj)
        {
            StatisticsByYearView.Close();
        }

        public bool CanExecute_CloseMonthStatisticsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CloseMonthStatisticsViewCommand = new RelayCommand(Executed_CloseMonthStatisticsViewCommand, CanExecute_CloseMonthStatisticsViewCommand);
            SelectYearForStatisticsCommand = new RelayCommand(Executed_SelectYearForStatisticsCommand, CanExecute_SelectYearForStatisticsCommand);
        }
    }
}

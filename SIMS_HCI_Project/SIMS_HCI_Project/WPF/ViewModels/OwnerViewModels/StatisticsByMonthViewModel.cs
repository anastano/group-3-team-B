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

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByMonthViewModel
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationStatisticsService _statisticsService;

        public StatisticsByMonthView StatisticsByMonthView { get; set; }
        public StatisticsByYearView StatisticsByYearView { get; set; }
        public SelectAccommodationForStatisticsView SelectAccommodationForStatisticsView { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationYear AccommodationYear { get; set; }
        public string BestMonth { get; set; }
        public List<AccommodationMonth> AccommodationMonths { get; set; }
        public RelayCommand CloseStatisticsByMonthViewCommand { get; set; }
        public RelayCommand HomeStatisticsByMonthViewCommand { get; set; }

        public StatisticsByMonthViewModel(StatisticsByMonthView statisticsByMonthView, StatisticsByYearView statisticsByYearView, 
            SelectAccommodationForStatisticsView selectAccommodationView, AccommodationReservationService reservationService, 
            AccommodationStatisticsService statisticsService, Accommodation selectedAccommodation, AccommodationYear accommodationYear)
        {
            InitCommands();

            _reservationService = reservationService;
            _statisticsService = statisticsService;

            StatisticsByMonthView = statisticsByMonthView;
            StatisticsByYearView = statisticsByYearView;
            SelectAccommodationForStatisticsView = selectAccommodationView;

            Accommodation = selectedAccommodation;
            AccommodationYear = accommodationYear;
            BestMonth = _statisticsService.FindBestMonthInYear(AccommodationYear.Year, Accommodation.Id);
            AccommodationMonths = _statisticsService.GetMonthsByAccommodationIdAndYear(Accommodation.Id, AccommodationYear.Year);
        }

        #region Commands

        public void Executed_CloseStatisticsByMonthViewCommand(object obj)
        {
            StatisticsByMonthView.Close();
        }

        public bool CanExecute_CloseStatisticsByMonthViewCommand(object obj)
        {
            return true;
        }

        public void Executed_HomeStatisticsByMonthViewCommand(object obj)
        {
            StatisticsByMonthView.Close();
            StatisticsByYearView.Close();
            SelectAccommodationForStatisticsView.Close();
        }

        public bool CanExecute_HomeStatisticsByMonthViewCommand(object obj)
        {
            return true;
        }

        #endregion

        public void InitCommands()
        {
            CloseStatisticsByMonthViewCommand = new RelayCommand(Executed_CloseStatisticsByMonthViewCommand, CanExecute_CloseStatisticsByMonthViewCommand);
            HomeStatisticsByMonthViewCommand = new RelayCommand(Executed_HomeStatisticsByMonthViewCommand, CanExecute_HomeStatisticsByMonthViewCommand);
        }
    }
}

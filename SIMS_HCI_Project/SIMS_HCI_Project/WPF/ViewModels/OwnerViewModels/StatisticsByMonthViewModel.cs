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
        private readonly AccommodationMonthStatisticsService _monthStatisticsService;
        public StatisticsByMonthView StatisticsByMonthView { get; set; }
        public StatisticsByYearView StatisticsByYearView { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationYear AccommodationYear { get; set; }
        public string BestMonth { get; set; }
        public List<AccommodationMonth> AccommodationMonths { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }

        public StatisticsByMonthViewModel(StatisticsByMonthView statisticsByMonthView, StatisticsByYearView statisticsByYearView,  Accommodation accommodation, AccommodationYear year)
        {
            InitCommands();

            _monthStatisticsService = new AccommodationMonthStatisticsService();

            StatisticsByMonthView = statisticsByMonthView;
            StatisticsByYearView = statisticsByYearView;

            Accommodation = accommodation;
            AccommodationYear = year;
            BestMonth = _monthStatisticsService.FindBestMonthInYear(AccommodationYear.Year, Accommodation.Id);
            AccommodationMonths = _monthStatisticsService.GetMonthsByAccommodationIdAndYear(Accommodation.Id, AccommodationYear.Year);
        }

        #region Commands
        public void Executed_CloseViewCommand(object obj)
        {
            StatisticsByMonthView.Close();
        }

        public void Executed_HomeViewCommand(object obj)
        {
            StatisticsByMonthView.Close();
            StatisticsByYearView.Close();
        }

        #endregion

        public void InitCommands()
        {
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            HomeViewCommand = new RelayCommand(Executed_HomeViewCommand);
        }
    }
}

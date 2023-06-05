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
        private readonly AccommodationYearStatisticsService _yearStatisticsService;
        public StatisticsByYearView StatisticsByYearView { get; set; }
        public SelectAccommodationForStatisticsView SelectAccommodationForStatisticsView { get; set; }
        public Accommodation Accommodation { get; set; }
        public int BestYear { get; set; }
        public List<AccommodationYear> AccommodationYears { get; set; }
        public AccommodationYear SelectedYear { get; set; }
        public RelayCommand SelectYearForStatisticsCommand { get; set; }
        public RelayCommand CloseStatisticsByYearViewCommand { get; set; }
        public RelayCommand HomeStatisticsByYearViewCommand { get; set; }

        public StatisticsByYearViewModel(StatisticsByYearView statisticsByYearView, SelectAccommodationForStatisticsView selectAccommodationView, Accommodation selectedAccommodation) 
        {
            InitCommands();

            _yearStatisticsService = new AccommodationYearStatisticsService();

            StatisticsByYearView = statisticsByYearView;
            SelectAccommodationForStatisticsView = selectAccommodationView;

            Accommodation = selectedAccommodation;
            BestYear = _yearStatisticsService.FindBestYear(Accommodation.Id);
            AccommodationYears = _yearStatisticsService.GetYearsByAccommodationId(Accommodation.Id);
        }

        #region Commands
        public void Executed_SelectYearForStatisticsCommand(object obj)
        {
            if (SelectedYear != null)
            {
                Window statisticsByMonth = new StatisticsByMonthView(StatisticsByYearView, SelectAccommodationForStatisticsView, Accommodation, SelectedYear);
                statisticsByMonth.ShowDialog();
            }
            else
            {
                MessageBox.Show("No year has been selected.");
            }
        }

        public bool CanExecute_SelectYearForStatisticsCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseStatisticsByYearViewCommand(object obj)
        {
            StatisticsByYearView.Close();
        }

        public bool CanExecute_CloseStatisticsByYearViewCommand(object obj)
        {
            return true;
        }

        public void Executed_HomeStatisticsByYearViewCommand(object obj)
        {
            StatisticsByYearView.Close();
            SelectAccommodationForStatisticsView.Close();
        }

        public bool CanExecute_HomeStatisticsByYearViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SelectYearForStatisticsCommand = new RelayCommand(Executed_SelectYearForStatisticsCommand, CanExecute_SelectYearForStatisticsCommand);
            CloseStatisticsByYearViewCommand = new RelayCommand(Executed_CloseStatisticsByYearViewCommand, CanExecute_CloseStatisticsByYearViewCommand);
            HomeStatisticsByYearViewCommand = new RelayCommand(Executed_HomeStatisticsByYearViewCommand, CanExecute_HomeStatisticsByYearViewCommand);
        }
    }
}

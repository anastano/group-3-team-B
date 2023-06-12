using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByYearViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationYearStatisticsService _yearStatisticsService;
        private readonly AccommodationService _accommodationService;
        public StatisticsByYearView StatisticsByYearView { get; set; }
        public Owner Owner { get; set; }
        public AccommodationYear SelectedYear { get; set; }
        public List<Accommodation> OwnerAccommodations { get; set; }

        #region OnPropertyChanged

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                if (value != _selectedAccommodation)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                    UpdateInfo();
                }
            }
        }

        private List<AccommodationYear> _accommodationYears;
        public List<AccommodationYear> AccommodationYears
        {
            get => _accommodationYears;
            set
            {
                if (value != _accommodationYears)
                {
                    _accommodationYears = value;
                    OnPropertyChanged(nameof(AccommodationYears));
                }
            }
        }

        private int _bestYear;
        public int BestYear
        {
            get => _bestYear;
            set
            {
                if (value != _bestYear)
                {
                    _bestYear = value;
                    OnPropertyChanged(nameof(BestYear));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand ShowMonthsCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }

        public StatisticsByYearViewModel(StatisticsByYearView statisticsByYearView, Owner owner) 
        {
            InitCommands();

            _yearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationService = new AccommodationService();

            StatisticsByYearView = statisticsByYearView;

            Owner = owner;
            OwnerAccommodations = _accommodationService.GetByOwnerId(Owner.Id);
            SelectedAccommodation = _accommodationService.GetByOwnerId(Owner.Id).FirstOrDefault();
            BestYear = _yearStatisticsService.FindBestYear(SelectedAccommodation.Id);
            AccommodationYears = _yearStatisticsService.GetYearsByAccommodationId(SelectedAccommodation.Id);
        }

        public void UpdateInfo() 
        {
            BestYear = _yearStatisticsService.FindBestYear(SelectedAccommodation.Id);
            AccommodationYears = _yearStatisticsService.GetYearsByAccommodationId(SelectedAccommodation.Id);
        }

        #region Commands
        public void Executed_ShowMonthsCommand(object obj)
        {
            if (SelectedYear != null)
            {
                Window statisticsByMonth = new StatisticsByMonthView(StatisticsByYearView, SelectedAccommodation, SelectedYear);
                statisticsByMonth.ShowDialog();
            }
            else
            {
                MessageBox.Show("No year has been selected.");
            }
        }


        public void Executed_CloseViewCommand(object obj)
        {
            StatisticsByYearView.Close();
        }

        public void Executed_HomeViewCommand(object obj)
        {
            StatisticsByYearView.Close();
        }

        #endregion

        public void InitCommands()
        {
            ShowMonthsCommand = new RelayCommand(Executed_ShowMonthsCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            HomeViewCommand = new RelayCommand(Executed_HomeViewCommand);
        }
    }
}

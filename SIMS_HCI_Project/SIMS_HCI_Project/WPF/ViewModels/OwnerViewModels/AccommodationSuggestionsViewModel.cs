using LiveCharts;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationSuggestionsViewModel : INotifyPropertyChanged
    {
        private readonly StatisticsForSuggestionsService _statisticsForSuggestionsService;
        public AccommodationSuggestionsView AccommodationSuggestionsView { get; set; }
        public AccommodationsView AccommodationsView { get; set; }
        public Owner Owner { get; set; }
        public LocationInfo BestLocation { get; set; }
        public ChartValues<double> BestLocationOccupancyPercentage { get; set; }
        public ChartValues<double> BestLocationNonOccupancyPercentage { get; set; }
        public LocationInfo WorstLocation { get; set; }
        public ChartValues<double> WorstLocationOccupancyPercentage { get; set; }
        public ChartValues<double> WorstLocationNonOccupancyPercentage { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }

        #region OnPropertyChanged

        private Style _closeButtonStyle;
        public Style CloseButtonStyle
        {
            get => _closeButtonStyle;
            set
            {
                if (value != _closeButtonStyle)
                {
                    _closeButtonStyle = value;
                    OnPropertyChanged(nameof(CloseButtonStyle));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public AccommodationSuggestionsViewModel(AccommodationSuggestionsView suggestionsView, AccommodationsView accommodationsView, Owner owner)
        {
            InitCommands();

            _statisticsForSuggestionsService = new StatisticsForSuggestionsService();

            AccommodationSuggestionsView = suggestionsView;
            AccommodationsView = accommodationsView;
            Owner = owner;

            BestLocation = _statisticsForSuggestionsService.FindBestLocationLastYear(Owner.Id);
            BestLocationOccupancyPercentage = new ChartValues<double> { Math.Round(BestLocation.OccupancyPercentageInLastYear ,2) };
            BestLocationNonOccupancyPercentage = new ChartValues<double> { Math.Round(100 - BestLocation.OccupancyPercentageInLastYear, 2)  };

            WorstLocation = _statisticsForSuggestionsService.FindWorstLocationLastYear(Owner.Id);
            WorstLocationOccupancyPercentage = new ChartValues<double> { Math.Round(WorstLocation.OccupancyPercentageInLastYear, 2) };
            WorstLocationNonOccupancyPercentage = new ChartValues<double> { Math.Round(100 - WorstLocation.OccupancyPercentageInLastYear, 2) };

            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            CloseButtonStyle = NormalButtonStyle;

            DemoIsOn();
        }

        #region DemoIsOn
        private async Task DemoIsOn()
        {
            if (OwnerMainViewModel.Demo)
            {
                await Task.Delay(2000);
                CloseButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500);
                CloseButtonStyle = NormalButtonStyle;
                AccommodationSuggestionsView.Close();

            }
        }
        #endregion

        #region Commands
        public void Executed_CloseViewCommand(object obj)
        {
            AccommodationSuggestionsView.Close();
        }

        public void Executed_HomeViewCommand(object obj)
        {
            AccommodationSuggestionsView.Close();
            AccommodationsView.Close();
        }

        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                //demo message - end
                AccommodationSuggestionsView.Close();
                AccommodationsView.Close();

                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();
            }
        }

        public void Executed_StopDemoCommand(object obj)
        {
            try
            {
                StopDemo();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }
        }

        #endregion

        public void InitCommands()
        {
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            HomeViewCommand = new RelayCommand(Executed_HomeViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand);
        }
    }
}

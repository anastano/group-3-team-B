using LiveCharts;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationSuggestionsViewModel
    {
        private readonly StatisticsForSuggestionsService _statisticsForSuggestionsService;
        public AccommodationSuggestionsView AccommodationSuggestionsView { get; set; }
        public AccommodationsView AccommodationsView { get; set; }
        public Owner Owner { get; set; }
        public LocationInfo BestLocation { get; set; }
        public ChartValues<double> OccupancyPercentageForBestLocation { get; set; }
        public ChartValues<double> NonOccupancyPercentageForBestLocation { get; set; }
        public LocationInfo WorstLocation { get; set; }
        public ChartValues<double> OccupancyPercentageForWorstLocation { get; set; }
        public ChartValues<double> NonOccupancyPercentageForWorstLocation { get; set; }
        public RelayCommand CloseAccommodationSuggestionsViewCommand { get; set; }
        public RelayCommand HomeAccommodationSuggestionsViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public AccommodationSuggestionsViewModel(AccommodationSuggestionsView accommodationSuggestionsView, AccommodationsView accommodationsView, Owner owner)
        {
            InitCommands();

            _statisticsForSuggestionsService = new StatisticsForSuggestionsService();

            AccommodationSuggestionsView = accommodationSuggestionsView;
            AccommodationsView = accommodationsView;
            Owner = owner;

            BestLocation = _statisticsForSuggestionsService.FindBestLocationLastYear(Owner.Id);
            OccupancyPercentageForBestLocation = new ChartValues<double> { Math.Round(BestLocation.OccupancyPercentageInLastYear ,2) };
            NonOccupancyPercentageForBestLocation = new ChartValues<double> { Math.Round(100 - BestLocation.OccupancyPercentageInLastYear, 2)  };

            WorstLocation = _statisticsForSuggestionsService.FindWorstLocationLastYear(Owner.Id);
            OccupancyPercentageForWorstLocation = new ChartValues<double> { Math.Round(WorstLocation.OccupancyPercentageInLastYear, 2) };
            NonOccupancyPercentageForWorstLocation = new ChartValues<double> { Math.Round(100 - WorstLocation.OccupancyPercentageInLastYear, 2) };

            DemoIsOn();
        }

        #region DemoIsOn
        private async Task DemoIsOn()
        {
            Style selectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            Style normalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            if (OwnerMainViewModel.Demo)
            {
                await Task.Delay(2000);
                AccommodationSuggestionsView.btnClose.Style = selectedButtonStyle;
                await Task.Delay(1500);
                AccommodationSuggestionsView.btnClose.Style = normalButtonStyle;
                AccommodationSuggestionsView.Close();

            }
        }
        #endregion

        #region Commands
        public void Executed_CloseAccommodationSuggestionsViewCommand(object obj)
        {
            AccommodationSuggestionsView.Close();
        }

        public bool CanExecute_CloseAccommodationSuggestionsViewCommand(object obj)
        {
            return true;
        }

        public void Executed_HomeAccommodationSuggestionsViewCommand(object obj)
        {
            AccommodationSuggestionsView.Close();
            AccommodationsView.Close();
        }

        public bool CanExecute_HomeAccommodationSuggestionsViewCommand(object obj)
        {
            return true;
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

        public bool CanExecute_StopDemoCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CloseAccommodationSuggestionsViewCommand = new RelayCommand(Executed_CloseAccommodationSuggestionsViewCommand, CanExecute_CloseAccommodationSuggestionsViewCommand);
            HomeAccommodationSuggestionsViewCommand = new RelayCommand(Executed_HomeAccommodationSuggestionsViewCommand, CanExecute_HomeAccommodationSuggestionsViewCommand);

            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
        }
    }
}

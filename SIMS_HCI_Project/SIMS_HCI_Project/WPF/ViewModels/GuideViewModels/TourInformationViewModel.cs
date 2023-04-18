using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourInformationViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand SeeStatistics { get; set; }
        public RelayCommand SeeReviews { get; set; }
        public RelayCommand CancelTour { get; set; }
        public RelayCommand SeeTourProgress { get; set; }
        #endregion

        public Tour Tour { get; set; }
        public TourTime SelectedTourTime { get; set; }

        private TourLifeCycleService _tourLifeCycleService;

        public TourInformationViewModel(Tour tour)
        {   
            Tour = tour;

            _tourLifeCycleService = new TourLifeCycleService(); 

            InitCommands();
        }

        private void InitCommands()
        {
            SeeStatistics = new RelayCommand(ExecutedSeeStatisticsCommand, CanExecuteCommand);
            SeeReviews = new RelayCommand(ExecutedSeeReviewsCommand, CanExecuteCommand);
            CancelTour = new RelayCommand(ExecutedCancelTourCommand, CanExecuteCommand);
            SeeTourProgress = new RelayCommand(ExecutedSeeTourProgressCommand, CanExecuteCommand);
        }

        private void ExecutedSeeStatisticsCommand(object obj)
        {

        }

        private void ExecutedSeeReviewsCommand(object obj)
        {

            Window tourReviews = new TourReviewsView();
            tourReviews.Show();
        }

        private void ExecutedCancelTourCommand(object obj)
        {

        }

        private void ExecutedSeeTourProgressCommand(object obj)
        {
            _tourLifeCycleService.StartTour(SelectedTourTime);
                
            Window tourProgress = new TourProgressView(SelectedTourTime);
            tourProgress.Show();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

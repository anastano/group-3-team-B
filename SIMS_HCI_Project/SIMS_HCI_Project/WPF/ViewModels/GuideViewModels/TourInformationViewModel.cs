using SIMS_HCI_Project.Applications.Services;
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

        public TourInformationViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            SeeStatistics = new RelayCommand(ExecutedSeeStatisticsCommand, CanExecuteCommand);
            SeeReviews = new RelayCommand(ExecutedSeeReviewsCommand, CanExecuteCommand);
            CancelTour = new RelayCommand(ExecutedCanceltourCommand, CanExecuteCommand);
            SeeTourProgress = new RelayCommand(ExecutedSeeTourProgressCommand, CanExecuteCommand);
        }

        private void ExecutedSeeStatisticsCommand(object obj)
        {

        }

        private void ExecutedSeeReviewsCommand(object obj)
        {

        }

        private void ExecutedCanceltourCommand(object obj)
        {

        }

        private void ExecutedSeeTourProgressCommand(object obj)
        {

        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

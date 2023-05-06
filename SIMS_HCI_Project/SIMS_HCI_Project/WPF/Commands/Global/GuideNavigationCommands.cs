using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.Commands.Global
{
    public class GuideNavigationCommands // this is class for now. Maybe move all commands to separate namespace individually. // breaks MVVM, idc // Maybe add NavService later
    {
        #region Commands
        public RelayCommand NavigateToHome { get; set; }
        public RelayCommand NavigateToAllTours { get; set; }
        public RelayCommand NavigateToTutorial { get; set; }
        public RelayCommand NavigateToTourInput { get; set; }
        public RelayCommand NavigateToTourInformation { get; set; }
        public RelayCommand NavigateToTourProgress { get; set; }
        public RelayCommand NavigateToReviews { get; set; }
        public RelayCommand NavigateToStatistics { get; set; }
        public RelayCommand NavigateToTourRequests { get; set; }
        public RelayCommand NavigateToTourRequestsStatistics { get; set; }
        public RelayCommand NavigateToProfileInformation { get; set; }
        public RelayCommand SignOut { get; set; }
        public RelayCommand CloseCurrentWindow { get; set; }
        #endregion

        public GuideNavigationCommands()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            NavigateToHome = new RelayCommand(ExecutedNavigateToHomeCommand, CanExecuteCommand);
            NavigateToAllTours = new RelayCommand(ExecutedNavigateToAllToursCommand, CanExecuteCommand);
            NavigateToTutorial = new RelayCommand(ExecutedNavigateToTutorialCommand, CanExecuteCommand);
            NavigateToTourInput = new RelayCommand(ExecutedNavigateToTourInputCommand, CanExecuteCommand);
            NavigateToTourInformation = new RelayCommand(ExecutedNavigateToTourInformationCommand, CanExecuteCommand);
            NavigateToTourProgress = new RelayCommand(ExecutedNavigateToTourProgressCommand, CanExecuteCommand);
            NavigateToReviews = new RelayCommand(ExecutedNavigateToReviewsCommand, CanExecuteCommand);
            NavigateToStatistics = new RelayCommand(ExecutedNavigateToStatisticsCommand, CanExecuteCommand);
            NavigateToTourRequests = new RelayCommand(ExecutedNavigateToTourRequestsCommand, CanExecuteCommand);
            NavigateToTourRequestsStatistics = new RelayCommand(ExecutedNavigateToTourRequestsStatisticsCommand, CanExecuteCommand);
            NavigateToProfileInformation = new RelayCommand(ExecutedNavigateToProfileInformationCommand, CanExecuteCommand);
            SignOut = new RelayCommand(ExecutedSignOutCommand, CanExecuteCommand);
            CloseCurrentWindow = new RelayCommand(ExecutedCloseCurrentWindowCommand, CanExecuteCommand);
        }

        private void ExecutedNavigateToHomeCommand(object obj)
        {
            Window home = new GuideMainView();
            home.Show();
        }

        private void ExecutedNavigateToAllToursCommand(object obj)
        {
            Window tours = new AllToursView(new TourService());
            tours.Show();;
        }

        private void ExecutedNavigateToTutorialCommand(object obj)
        {

        }

        private void ExecutedNavigateToTourInputCommand(object obj)
        {
            Window tourInput = new TourInputView(new TourService());
            tourInput.Show();
        }

        private void ExecutedNavigateToTourInformationCommand(object obj)
        {
            Window tourInfo = new TourInformationView(obj as Tour);
            tourInfo.Show();
        }

        private void ExecutedNavigateToTourProgressCommand(object obj)
        {
            Window tours = new TourProgressView(obj as TourTime);
            tours.Show();
        }

        private void ExecutedNavigateToReviewsCommand(object obj)
        {
            Window tours = new TourReviewsView(obj as TourTime);
            tours.Show();
        }

        private void ExecutedNavigateToStatisticsCommand(object obj)
        {
            Window tourStatistics = new AllToursStatisticsView();
            tourStatistics.Show();
        }

        private void ExecutedNavigateToTourRequestsCommand(object obj)
        {
            Window tourRequests = new TourRequestsView();
            tourRequests.Show();
        }
        
        private void ExecutedNavigateToTourRequestsStatisticsCommand(object obj)
        {
            Window tourRequestsStatistics = new TourRequestsStatisticsView();
            tourRequestsStatistics.Show();
        }
        
        private void ExecutedNavigateToProfileInformationCommand(object obj)
        {
            Window profile = new MyProfileView();
            profile.Show();
        }

        private void ExecutedSignOutCommand(object obj)
        {
            Window logIn = new LoginWindow();
            for (int i = App.Current.Windows.Count - 1; i >= 0; i--)
            {
                if (App.Current.Windows[i] == logIn) continue;
                App.Current.Windows[i].Close();
            }
            logIn.Show();
        }

        private void ExecutedCloseCurrentWindowCommand(object obj)
        {
            if (obj != null)
            {
                ((Window)obj).Close();
            }
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

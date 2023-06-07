﻿using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Commands.Global;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public RelayCommand StartTour { get; set; }
        public RelayCommand SeeTourProgress { get; set; }
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion

        public Tour Tour { get; set; }

        private string _mainImage;
        public string MainImage
        {
            get { return _mainImage; }
            set
            {
                _mainImage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _allImages;
        public ObservableCollection<string> AllImages
        {
            get { return _allImages; }
            set
            {
                _allImages = value;
                OnPropertyChanged();
            }
        }

        private TourTime _selectedTourTime;
        public TourTime SelectedTourTime
        {
            get { return _selectedTourTime; }
            set
            {
                _selectedTourTime = value;
                OnPropertyChanged();
            }
        }

        private TourLifeCycleService _tourLifeCycleService;

        public TourTime AllTimeTopTour { get; set; }

        public TourStatisticsInfo AllTimeTopTourStatistics { get; set; }
        private TourStatisticsService _tourStatisticsService;

        public TourInformationViewModel(Tour tour)
        {   
            Tour = tour;

            _tourStatisticsService = new TourStatisticsService();
            AllTimeTopTour = _tourStatisticsService.GetTopTour();
            AllTimeTopTourStatistics = _tourStatisticsService.GetTourStatistics(AllTimeTopTour.Id);

            if (tour.DepartureTimes != null && tour.DepartureTimes.Count > 0)
            {
                SelectedTourTime = tour.DepartureTimes.First();
            }

            _tourLifeCycleService = new TourLifeCycleService(); 

            InitCommands();
            LoadImages();
        }

        private void LoadImages()
        {
            AllImages = new ObservableCollection<string>(Tour.Images);
            MainImage = Tour.Images.First();
        }

        private void InitCommands()
        {
            SeeStatistics = new RelayCommand(ExecutedSeeStatisticsCommand, CanExecuteCommand);
            CancelTour = new RelayCommand(ExecutedCancelTourCommand, CanExecuteCommand);
            StartTour = new RelayCommand(ExecutedStartTourCommand, CanExecuteCommand);
            NavigationCommands = new GuideNavigationCommands();
        }

        private void ExecutedSeeStatisticsCommand(object obj)
        {
            Window tourStatistics = new TourStatisticsView(SelectedTourTime);
            tourStatistics.Show();
        }

        private void ExecutedCancelTourCommand(object obj)
        {
            string messageBoxText = "Are you sure you want to cancel tour? This action cannot be undone";
            string caption = "Cancel Tour";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                _tourLifeCycleService.CancelTour(SelectedTourTime);
            }
        }

        private void ExecutedStartTourCommand(object obj)
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

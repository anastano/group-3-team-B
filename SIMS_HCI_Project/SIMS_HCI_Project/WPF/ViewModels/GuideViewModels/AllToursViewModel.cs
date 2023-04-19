using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class AllToursViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand AddNewTour { get; set; }
        public RelayCommand SeeTourInfo { get; set; }
        #endregion

        private ObservableCollection<Tour> _allTours;
        public ObservableCollection<Tour> AllTours
        {
            get { return _allTours; }
            set
            {
                _allTours = value;
                OnPropertyChanged();
            }
        }

        public Tour SelectedTour { get; set; }

        private TourService _tourService;

        private Guide _guide;

        public AllToursViewModel(TourService tourService, Guide guide)
        {
            _tourService = tourService;
            _guide = guide;

            LoadAllTours();
            InitCommands();
        }

        private void LoadAllTours()
        {
            AllTours = new ObservableCollection<Tour>(_tourService.GetAllTourInformationByGuide(_guide.Id));
        }

        private void InitCommands()
        {
            AddNewTour = new RelayCommand(ExecutedAddNewTourCommand, CanExecuteCommand);
            SeeTourInfo = new RelayCommand(ExecutedSeeTourInfoCommand, CanExecuteCommand);
        }

        private void ExecutedAddNewTourCommand(object obj)
        {
            Window tourInput = new TourInputView(_tourService, _guide);
            tourInput.Show();
        }

        private void ExecutedSeeTourInfoCommand(object obj)
        {
            Trace.Write("aaaa");
            Window tourInfo = new TourInformationView(SelectedTour);
            tourInfo.Show();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

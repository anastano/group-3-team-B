using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Commands.Global;
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
        public GuideNavigationCommands NavigationCommands { get; set; }
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

        public AllToursViewModel(TourService tourService)
        {
            _tourService = tourService;

            LoadAllTours();
            InitCommands();
        }

        private void LoadAllTours()
        {
            AllTours = new ObservableCollection<Tour>(_tourService.GetAllTourInformationByGuide(((User)App.Current.Properties["CurrentUser"]).Id));
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
        }
    }
}

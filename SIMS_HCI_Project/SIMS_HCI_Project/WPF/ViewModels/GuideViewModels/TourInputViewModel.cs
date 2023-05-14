using SIMS_HCI_Project.Applications.Services;
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
using System.Windows.Input;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourInputViewModel: INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand AddKeyPoint { get; set; }
        public RelayCommand RemoveKeyPoint { get; set; }
        public RelayCommand AddDepartureTime { get; set; }
        public RelayCommand RemoveDepartureTime { get; set; }
        public RelayCommand AddImage { get; set; }
        public RelayCommand RemoveImage { get; set; }
        public RelayCommand SubmitForm { get; set; }
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion

        #region newDepartureDate
        private DateTime _departureDate;
        public DateTime DepartureDate
        {
            get { return _departureDate; }
            set
            {
                _departureDate = value;
                OnPropertyChanged();
            }
        }
        private TimeOnly _departureTime;
        public TimeOnly DepartureTime
        {
            get { return _departureTime; }
            set
            {
                _departureTime = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<DateTime> DepartureTimes { get; set; }
        public DateTime SelectedDepartureTime { get; set; }

        #region newKeyPoint
        private string _keyPointTitle;
        public string KeyPointTitle
        {
            get { return _keyPointTitle; }
            set
            {
                _keyPointTitle = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }
        public TourKeyPoint SelectedKeyPoint { get; set; }

        #region newImage
        private string _imageURL;
        public string ImageURL
        {
            get { return _imageURL; }
            set
            {
                _imageURL = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ObservableCollection<string> Images { get; set; }
        public string SelectedImage { get; set; }

        private readonly TourService _tourService;

        private Tour _newTour;
        public Tour NewTour
        {
            get { return _newTour; }
            set
            {
                _newTour = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubmitButtonEnabled;
        public bool IsSubmitButtonEnabled
        {
            get { return _isSubmitButtonEnabled; }
            set
            {
                _isSubmitButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public TourInputViewModel(Tour tour = null)
        {
            _tourService = new TourService();

            NewTour = tour == null ? new Tour(new Guide(((User)App.Current.Properties["CurrentUser"]))) : tour;

            DepartureDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            DepartureTime = new TimeOnly();

            Images = new ObservableCollection<string>();
            DepartureTimes = new ObservableCollection<DateTime>();
            KeyPoints = new ObservableCollection<TourKeyPoint>();

            InitCommands();
        }

        private void InitCommands()
        {
            AddKeyPoint = new RelayCommand(ExecutedAddKeyPointCommand, CanExecuteCommand);
            RemoveKeyPoint = new RelayCommand(ExecutedRemoveKeyPointCommand, CanExecuteCommand);
            AddDepartureTime = new RelayCommand(ExecutedAddDepartureTimeCommand, CanExecuteCommand);
            RemoveDepartureTime = new RelayCommand(ExecutedRemoveDepartureTimeCommand, CanExecuteCommand);
            AddImage = new RelayCommand(ExecutedAddImageCommand, CanExecuteCommand);
            RemoveImage = new RelayCommand(ExecutedRemoveImageCommand, CanExecuteCommand);
            SubmitForm = new RelayCommand(ExecutedSubmitFormCommand, CanExecuteCommand);
            NavigationCommands = new GuideNavigationCommands();
        }

        private void ExecutedAddKeyPointCommand(object obj)
        {
            if (KeyPointTitle != "")
            {
                KeyPoints.Add(new TourKeyPoint(KeyPointTitle));
                NewTour.KeyPoints.Add(new TourKeyPoint(KeyPointTitle));
                KeyPointTitle = "";
            }
        }

        private void ExecutedRemoveKeyPointCommand(object obj)
        {
        }

        private void ExecutedAddDepartureTimeCommand(object obj)
        {
            DateTime newDepartureTime = DepartureDate;
            newDepartureTime += DepartureTime.ToTimeSpan();

            DepartureTimes.Add(newDepartureTime);
            NewTour.DepartureTimes.Add(new TourTime(newDepartureTime));

            DepartureDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            DepartureTime = new TimeOnly();
        }

        private void ExecutedRemoveDepartureTimeCommand(object obj)
        {
        }

        private void ExecutedAddImageCommand(object obj)
        {
            if (ImageURL != "")
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }

        private void ExecutedRemoveImageCommand(object obj)
        {
        }

        private void ExecutedSubmitFormCommand(object obj)
        {
            NewTour.Images.AddRange(Images);
            _tourService.Add(NewTour);
            //todo: add function for notifications NEW_TOUR, ide kroz sve zahteve i ako tura sadrzi jezik ili id lokacije kao u zahtevu pravi obavestenje
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

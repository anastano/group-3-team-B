using SIMS_HCI_Project.WPF.Commands.Global;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    public class ComplexTourRequestsViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand FilterRequests { get; set; }
        public RelayCommand ResetFilter { get; set; }
        public RelayCommand AcceptRequest { get; set; }
        public RelayCommand ConfirmPickedDate { get; set; }
        public RelayCommand CreateTourFromTopLanguage { get; set; }
        public RelayCommand CreateTourFromTopLocation { get; set; }
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion


        public Tour Tour { get; set; }

        private ComplexTourRequestsService _complexTourRequestsService;

        private ObservableCollection<ComplexTourRequest> _complexRequests;
        public ObservableCollection<ComplexTourRequest> ComplexRequests
        {
            get { return _complexRequests; }
            set
            {
                _complexRequests = value;
                OnPropertyChanged();
            }
        }

        private ComplexTourRequest _selectedComplex;
        public ComplexTourRequest SelectedComplex
        {
            get { return _selectedComplex; }
            set
            {
                _selectedComplex = value;
                HasAcceptedPart = _selectedComplex.HasAcceptedPart(guide.Id);

                OnPropertyChanged();
            }
        }

        private RegularTourRequest _selectedTourRequest;
        public RegularTourRequest SelectedTourRequest
        {
            get { return _selectedTourRequest; }
            set
            {
                _selectedTourRequest = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DateTime> _possibleDepartureTimes;
        public ObservableCollection<DateTime> PossibleDepartureTimes
        {
            get { return _possibleDepartureTimes; }
            set
            {
                _possibleDepartureTimes = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDepartureTime;
        public DateTime SelectedDepartureTime
        {
            get { return _selectedDepartureTime; }
            set
            {
                _selectedDepartureTime = value;
                OnPropertyChanged();
            }
        }

        private Guide guide;

        private bool _hasAcceptedPart;
        public bool HasAcceptedPart
        {
            get { return _hasAcceptedPart; }
            set
            {
                _hasAcceptedPart = value;
                OnPropertyChanged();
            }
        }
        
        public ComplexTourRequestsViewModel() 
        {
            _complexTourRequestsService = new ComplexTourRequestsService();

            InitCommands();
            LoadRequests();

            guide = (Guide)App.Current.Properties["CurrentUser"];

            if (ComplexRequests != null && ComplexRequests.Count > 0)
            {
                SelectedComplex = ComplexRequests.First();
                HasAcceptedPart = SelectedComplex.HasAcceptedPart(guide.Id);
            }
        }

        private void InitCommands()
        {
            ConfirmPickedDate = new RelayCommand(ExecutedConfirmPickedDateCommand, CanExecuteCommand);
            AcceptRequest = new RelayCommand(ExecutedAcceptRequestCommand, CanExecuteCommand);
            NavigationCommands = new GuideNavigationCommands();
        }

        private void LoadRequests()
        {
            ComplexRequests = new ObservableCollection<ComplexTourRequest>(_complexTourRequestsService.GetAll());
        }

        private void ExecutedAcceptRequestCommand(object obj)
        {
            PossibleDepartureTimes = new ObservableCollection<DateTime>(_complexTourRequestsService.GeneratePossibleDepartureTimes(SelectedTourRequest, guide));
        }

        private void ExecutedConfirmPickedDateCommand(object obj)
        {
            Tour = _complexTourRequestsService.AcceptRequest(SelectedTourRequest, guide, SelectedDepartureTime);
            if (Tour == null)
            {
                MessageBox.Show("You already have tour in that time slot or someone else took that time slot.", "Acceptance failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }
            LoadRequests(); 
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

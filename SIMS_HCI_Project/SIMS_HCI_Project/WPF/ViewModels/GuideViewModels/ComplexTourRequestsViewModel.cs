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
        public DateTime PickedDate { get; set; }
        public DateTime PickedTime { get; set; }

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

        public ComplexTourRequestsViewModel() 
        {
            _complexTourRequestsService = new ComplexTourRequestsService();

            PickedDate = DateTime.Now;

            InitCommands();
            LoadRequests();

            if(ComplexRequests != null && ComplexRequests.Count > 0)
            {
                SelectedComplex = ComplexRequests.First();
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
            PickedDate = SelectedTourRequest.DateRange.Start;
        }

        private void ExecutedConfirmPickedDateCommand(object obj)
        {
            Tour = _complexTourRequestsService.AcceptRequest(SelectedTourRequest, ((User)App.Current.Properties["CurrentUser"]).Id, new DateTime(PickedDate.Year, PickedDate.Month, PickedDate.Day, PickedTime.Hour, PickedTime.Minute, 0));
            if (Tour == null)
            {
                MessageBox.Show("You already have tour in that time slot.", "Acceptance failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }
            LoadRequests();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Windows.Controls;


namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class RegularRequestsViewModel : INotifyPropertyChanged

    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public RelayCommand CreateRequest { get; set; }
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }
        public RegularRequestsView RegularRequestsView { get; set; }
        public Guest2 Guest { get; set; }

        private ObservableCollection<RegularTourRequest> _myRequests;
        public ObservableCollection<RegularTourRequest> MyRequests
        {
            get { return _myRequests; }
            set
            {
                _myRequests = value;
                OnPropertyChanged();
            }
        }

        private Frame _createRequestFrame;
        public Frame CreateRequestFrame
        {
            get { return _createRequestFrame; }
            set
            {
                _createRequestFrame = value;
                OnPropertyChanged();
            }
        }

        public RegularRequestsViewModel(RegularRequestsView regularRequestsView, Guest2 guest2, NavigationService navigationService, Frame createRequestFrame)
        {
            RegularRequestsView = regularRequestsView;
            Guest = guest2;
            NavigationService = navigationService;
            CreateRequestFrame = createRequestFrame;

            LoadFromFiles();
            InitCommands();

            MyRequests = new ObservableCollection<RegularTourRequest>(_regularTourRequestService.GetAllByGuestIdNotPartOfComplex(Guest.Id)); //getallbyguestid (ali one koje nisu deo slozene, dodaj to kao flag)
        }

        private void InitCommands()
        {
            CreateRequest = new RelayCommand(ExecuteCreateRequest, CanExecute);
        }

        private void ExecuteCreateRequest(object obj)
        {
            CreateRequestFrame.NavigationService.Navigate(new CreateRegularRequestView(Guest, NavigationService));
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;
using System.ComponentModel;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class CreateRegularRequestViewModel : INotifyPropertyChanged
    {
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }
        public Guest2 Guest { get; set; }
        public RegularTourRequest RegularTourRequest { get; set; }
        public RelayCommand CompleteRequest { get; set; }
        public RelayCommand QuitRequest { get; set; }
        public CreateRegularRequestView CreateRegularRequestView { get; set; }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _end;
        public DateTime End
        {
            get => _end;
            set
            {
                if (value != _end)
                {
                    _end = value;
                    OnPropertyChanged();
                }
            }
        }

        public CreateRegularRequestViewModel(Guest2 guest, NavigationService navigationService, CreateRegularRequestView createRegularRequestView)
        {
            Guest = guest;
            NavigationService = navigationService;
            CreateRegularRequestView = createRegularRequestView;
            RegularTourRequest = new RegularTourRequest();
            RegularTourRequest.Location = new Location();

            Start = DateTime.Now.AddDays(3); //48h before requests turns invalid
            End = DateTime.Now.AddDays(4);

            InitCommands();
            LoadFromFiles();

        }

        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
        }

        private void InitCommands()
        {
            CompleteRequest = new RelayCommand(ExecuteCompleteRequest, CanExecute);
            QuitRequest = new RelayCommand(ExecuteQuitRequest, CanExecute);
        }

        private void ExecuteQuitRequest(object obj)
        {
            NavigationService.Navigate(new RequestsView(Guest, NavigationService));
        }

        private void ExecuteCompleteRequest(object obj)
        {
            RegularTourRequest.GuestId = Guest.Id;
            RegularTourRequest.Guest = Guest;
            RegularTourRequest.IsPartOfComplex = false;
            RegularTourRequest.SubmittingDate = DateTime.Now.AddDays(0);
            RegularTourRequest.Status = RegularRequestStatus.PENDING;
            RegularTourRequest.Start = Start;
            RegularTourRequest.End = End;

            _regularTourRequestService.Add(RegularTourRequest);
            MessageBox.Show("Tour request is submited. You can see it in the list of your regular tour requests list.");
            NavigationService.Navigate(new RequestsView(Guest, NavigationService));
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RequestViewModel : INotifyPropertyChanged
    {
        private readonly RescheduleRequestService _requestService;
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;

        public RequestView RequestView { get; set; }
        public RequestsViewModel RequestsVM { get; set; }
        public RescheduleRequest Request { get; set; }
        public ObservableCollection<AccommodationReservation> OverlappingReservations { get; set; }

        #region OnPropertyChanged

        private string _overlappingReservationsText;
        public string OverlappingReservationsText
    {
            get => _overlappingReservationsText;
            set
            {
                if (value != _overlappingReservationsText)
                {
                    _overlappingReservationsText = value;
                    OnPropertyChanged(nameof(OverlappingReservationsText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand DeclineCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }

        public RequestViewModel(RequestView requestHandlerView, RequestsViewModel requestsVM, RescheduleRequest request) 
        {
            InitCommands();

            _requestService = new RescheduleRequestService();
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();

            RequestView = requestHandlerView;
            RequestsVM = requestsVM;
            Request = request;
            
            OverlappingReservations = new ObservableCollection<AccommodationReservation>(_requestService.GetOverlappingReservations(Request, _reservationService));

            ShowTextBox();
        }

        #region Commands

        private MessageBoxResult ConfirmAcceptRequest()
        {
            string sMessageBoxText = $"Are you sure you want to accept this request?";
            string sCaption = "Accept Request Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_AcceptCommand(object obj)
        {
            if (ConfirmAcceptRequest() == MessageBoxResult.Yes)
            {
                if (OverlappingReservations != null)
                {
                    foreach (AccommodationReservation reservation in OverlappingReservations)
                    {
                        _reservationService.EditStatus(reservation.Id, AccommodationReservationStatus.CANCELLED);
                    }
                }

                _reservationService.EditReservation(Request);
                _requestService.EditStatus(Request.Id, RescheduleRequestStatus.ACCEPTED);

                String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been ACCEPTED";
                _notificationService.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

                RequestView.Close();
                RequestsVM.UpdatePendingRequests();
            }
        }

        public void Executed_DeclineCommand(object obj)
        {
            Window requestDenialView = new DeclineRequestView(RequestView, RequestsVM, Request);
            requestDenialView.Show();
        }

        public void Executed_CloseViewCommand(object obj)
        {
            RequestView.Close();
        }

        #endregion

        public void InitCommands()
        {
            AcceptCommand = new RelayCommand(Executed_AcceptCommand);
            DeclineCommand = new RelayCommand(Executed_DeclineCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }

        public void ShowTextBox()
        {
            int overlappingReservations = _requestService.GetOverlappingReservations(Request, _reservationService).Count;
            if (overlappingReservations != 0)
            {
                OverlappingReservationsText = "There are reservations on those days: ";
            }
            else
            {
                OverlappingReservationsText = "There are not any reservations on those days.";
            }

        }
    }
}

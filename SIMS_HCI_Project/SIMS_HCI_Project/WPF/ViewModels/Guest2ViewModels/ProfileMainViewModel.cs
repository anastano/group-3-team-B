using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf.draw;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ProfileMainViewModel : INotifyPropertyChanged
    {
        //912, 518
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        #region Services
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private NotificationService _notificationService;
        #endregion

        #region Properties

        public List<GuestTourAttendance> Attendances { get; set; }
        public GuestTourAttendance GuestTourAttendance { get; set; }

        private int _unreadCount;
        public int UnreadCount
        {
            get { return _unreadCount; }
            set
            {
                _unreadCount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TourReservation> _reservations;
        public ObservableCollection<TourReservation> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TourVoucher> _vouchers;
        public ObservableCollection<TourVoucher> Vouchers
        {
            get { return _vouchers; }
            set
            {
                _vouchers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TourReservation> _activeTours;
        public ObservableCollection<TourReservation> ActiveTours
        {
            get { return _activeTours; }
            set
            {
                _activeTours = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public string UnreadNotificationsMessage { get; set; }
        public Frame ProfileFrame { get; set; }
        #region Commands
        public RelayCommand ShowNotificationsCommand { get; set; }
        public RelayCommand DownloadPDFReportCommand { get; set; }
        //TODO HELP CMD

        #endregion
        public ProfileMainViewModel(Guest2 guest, NavigationService navigationService, Frame profileFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            ProfileFrame = profileFrame;
            InitCommands();
            LoadFromFiles();
            ShowNotificationsCount();

            Attendances = new List<GuestTourAttendance>(); //trebace kada pravi obavestenja da potvrdi attendance na turi, sredi drugacije nekako, MakeNotificationsForAttendanceConfirmation() u Guest2MainViewModel 
            GuestTourAttendance = new GuestTourAttendance(); //ne znam sta ce, mozda u xamlu? vidi?

            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));

            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));

            ActiveTours = new ObservableCollection<TourReservation>(_tourReservationService.GetActiveByGuestId(guest.Id));

            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id)); //izmeni da prosledi sve notifikacije tog gosta u novi prof Notif page, a ne samo neporcitane,
                                                                                                                      //ili ovo ne treba uopste, vec da samo prikaze broj enporcitanih, a u narednom ce sam napraviti ovu listu notifikacija na osnovu id gosta
                                                                                                                      //a da u ovom page prikaze broj neprocitanih

            //sredi xaml kod za ovaj frame
            //MakeNotificationsForAttendanceConfirmation();
            //makeNotifPls();
        }

        public void ShowNotificationsCount()
        {
            //uradi kao labela bindovana
            UnreadCount = _notificationService.GetUnreadByUserId(Guest.Id).Count();
            UnreadNotificationsMessage = "You have " + UnreadCount + " unread notifications.";
        }


        private void LoadFromFiles()
        {
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _notificationService = new NotificationService(); //proveri da li je u startap servisu sve povezano za obavestenja 
        }

        private void InitCommands()
        {
            ShowNotificationsCommand = new RelayCommand(ExecuteShowNotifications, CanExecute);
            DownloadPDFReportCommand = new RelayCommand(ExecuteGenerateVoucherReport, CanExecute);
        }

        private void ExecuteShowNotifications(object obj)
        {
            ProfileFrame.NavigationService.Navigate(new ProfileNotificationsView(Guest, NavigationService, ProfileFrame)); //check if ok?
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void ExecuteGenerateVoucherReport(object obj)
        {
            string date = DateTime.Now.ToShortDateString();

            Document document = new Document();
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream("VoucherReport_" + Guest.Name + "_" + Guest.Surname + "_" +date +".pdf", FileMode.Create));
            document.Open();

            Paragraph guestName = new Paragraph("Guest: " + Guest.Name + " " + Guest.Surname);
            guestName.Alignment = Element.ALIGN_LEFT;
            guestName.SpacingAfter = 2f;
            guestName.SpacingBefore = 5f;
            guestName.Font = FontFactory.GetFont(FontFactory.HELVETICA, 13);
            document.Add(guestName);

            Paragraph creationDate = new Paragraph("Date: " + date);
            creationDate.Alignment = Element.ALIGN_LEFT;
            creationDate.SpacingAfter = 20f;
            creationDate.Font = FontFactory.GetFont(FontFactory.HELVETICA, 13);
            document.Add(creationDate);

            Paragraph text = new Paragraph("You can see a report of all currently valid tourist vouchers for the guest " + Guest.Name + " " + Guest.Surname + ".");
            text.Alignment = Element.ALIGN_LEFT;
            text.SpacingAfter = 2f;
            text.SpacingBefore = 5f;
            text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 13);
            document.Add(text);

            Paragraph voucherInfo = new Paragraph("Voucher report");
            voucherInfo.Alignment = Element.ALIGN_CENTER;
            voucherInfo.SpacingBefore = 25f;
            voucherInfo.SpacingAfter = 5f;
            voucherInfo.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 17);
            document.Add(voucherInfo);

            float[] widths = { 20, 20, 20, 20, 20 };
            PdfPTable table = new PdfPTable(widths);
            table.DefaultCell.FixedHeight = 38f;

            PdfPCell header1 = new PdfPCell(new Phrase("Voucher ID"));
            header1.HorizontalAlignment = Element.ALIGN_CENTER;
            header1.VerticalAlignment = Element.ALIGN_MIDDLE;
            header1.FixedHeight = 40f;

            PdfPCell header2 = new PdfPCell(new Phrase("Title"));
            header2.HorizontalAlignment = Element.ALIGN_CENTER;
            header2.VerticalAlignment = Element.ALIGN_MIDDLE;
            header2.FixedHeight = 40f;

            PdfPCell header3 = new PdfPCell(new Phrase("Acquired Date"));
            header3.HorizontalAlignment = Element.ALIGN_CENTER;
            header3.VerticalAlignment = Element.ALIGN_MIDDLE;
            header3.FixedHeight = 40f;

            PdfPCell header4 = new PdfPCell(new Phrase("Expiration Date"));
            header4.HorizontalAlignment = Element.ALIGN_CENTER;
            header4.VerticalAlignment = Element.ALIGN_MIDDLE;
            header4.FixedHeight = 40f;

            PdfPCell header5 = new PdfPCell(new Phrase("Status"));
            header5.HorizontalAlignment = Element.ALIGN_CENTER;
            header5.VerticalAlignment = Element.ALIGN_MIDDLE;
            header5.FixedHeight = 40f;

            table.AddCell(header1);
            table.AddCell(header2);
            table.AddCell(header3);
            table.AddCell(header4);
            table.AddCell(header5);

            
             foreach (TourVoucher voucher in Vouchers)
            {
                PdfPCell voucherIdCell = new PdfPCell(new Phrase(voucher.Id.ToString()));
                voucherIdCell.HorizontalAlignment = Element.ALIGN_CENTER;
                voucherIdCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                voucherIdCell.FixedHeight = 38f;

                PdfPCell titleCell = new PdfPCell(new Phrase(voucher.Title));
                titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                titleCell.FixedHeight = 38f;

                PdfPCell acquiredDateCell = new PdfPCell(new Phrase(voucher.AquiredDate.ToString()));
                acquiredDateCell.HorizontalAlignment = Element.ALIGN_CENTER;
                acquiredDateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                acquiredDateCell.FixedHeight = 38f;

                PdfPCell expirationDateCell = new PdfPCell(new Phrase(voucher.ExpirationDate.ToString()));
                expirationDateCell.HorizontalAlignment = Element.ALIGN_CENTER;
                expirationDateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                expirationDateCell.FixedHeight = 38f;

                PdfPCell statusCell = new PdfPCell(new Phrase(voucher.Status.ToString()));
                statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                statusCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                statusCell.FixedHeight = 38f;

                table.AddCell(voucherIdCell);
                table.AddCell(titleCell);
                table.AddCell(acquiredDateCell);
                table.AddCell(expirationDateCell);
                table.AddCell(statusCell);
            }

            document.Add(table);
            document.Close();

            MessageBox.Show("Report successfully created.");
        }

    }
}

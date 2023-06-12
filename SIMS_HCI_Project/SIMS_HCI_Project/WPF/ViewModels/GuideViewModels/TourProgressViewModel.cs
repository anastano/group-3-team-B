using iTextSharp.text.pdf;
using iTextSharp.text;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using iTextSharp.text.pdf.draw;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourProgressViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand MarkGuestPresent { get; set; }
        public RelayCommand EndTour { get; set; }
        public RelayCommand MoveKeyPoint { get; set; }
        public RelayCommand DownloadPDF { get; set; }
        #endregion

        private TourTime _tour;
        public TourTime Tour
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged();
            }
        }

        public GuestTourAttendance SelectedGuest { get; set; }

        private TourService _tourService;
        private TourLifeCycleService _tourLifeCycleService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private NotificationService _notificationService;

        public TourProgressViewModel(TourTime tour)
        {
            Tour = tour;

            _tourLifeCycleService = new TourLifeCycleService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourService = new TourService();
            _notificationService = new NotificationService();

            InitCommands();
        }

        private void InitCommands()
        {
            MarkGuestPresent = new RelayCommand(ExecutedMarkGuestPresentCommand, CanExecuteCommand);
            EndTour = new RelayCommand(ExecutedEndTourCommand, CanExecuteCommand);
            MoveKeyPoint = new RelayCommand(ExecutedMoveKeyPointCommand, CanExecuteCommand);
            DownloadPDF = new RelayCommand(ExecutedDownloadPDFCommand, CanExecuteCommand);
        }

        private void ExecutedMarkGuestPresentCommand(object obj)
        {
            _guestTourAttendanceService.MarkGuestAsPresent(SelectedGuest);
            LoadTour();
        }

        private void ExecutedEndTourCommand(object obj)
        {
            string messageBoxText = "Are you sure you want to end tour? This action cannot be undone";
            string caption = "End Tour";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if(result == MessageBoxResult.Yes)
            {
                _tourLifeCycleService.EndTour(Tour);
                LoadTour();
            }
        }

        private void ExecutedMoveKeyPointCommand(object obj)
        {
            _tourLifeCycleService.MoveToNextKeyPoint(Tour);
            LoadTour();
        }

        private void ExecutedDownloadPDFCommand(object obj)
        {
            Document document = new Document();
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream("Tour" + "-" + Tour.Id + ".pdf", FileMode.Create));
            document.Open();

            Paragraph header = new Paragraph("GUEST LIST FOR TOUR " + Tour.Tour.Title);
            header.Alignment = Element.ALIGN_CENTER;
            header.SpacingAfter = 5f;
            header.Font = FontFactory.GetFont(FontFactory.HELVETICA, 15);
            document.Add(header);

            LineSeparator ls = new LineSeparator();
            document.Add(ls);

            Paragraph tourInformation = new Paragraph("Tour information: ");
            tourInformation.Alignment = Element.ALIGN_LEFT;
            tourInformation.SpacingAfter = 2f;
            tourInformation.SpacingBefore = 25f;
            tourInformation.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            document.Add(tourInformation);

            Paragraph tourTitle = new Paragraph(Tour.Tour.Title);
            tourTitle.Alignment = Element.ALIGN_LEFT;
            tourTitle.SpacingAfter = 2f;
            tourTitle.SpacingBefore = 3f;
            tourTitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 13);
            document.Add(tourTitle);

            Paragraph tourInfo = new Paragraph(Tour.Tour.Location.City + ", " + Tour.Tour.Location.Country);
            tourTitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            tourInfo.Alignment = Element.ALIGN_LEFT;
            tourInfo.SpacingAfter = 2f;
            document.Add(tourInfo);

            tourInfo = new Paragraph(Tour.DepartureTime.ToString());
            tourTitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            tourInfo.Alignment = Element.ALIGN_LEFT;
            tourInfo.SpacingAfter = 2f;
            document.Add(tourInfo);

            tourInfo = new Paragraph(Tour.Tour.Language);
            tourTitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            tourInfo.Alignment = Element.ALIGN_LEFT;
            tourInfo.SpacingAfter = 20f;
            document.Add(tourInfo);

            float[] widths = { 10, 10, 10 };
            PdfPTable table = new PdfPTable(widths);
            table.DefaultCell.FixedHeight = 38f;

            PdfPCell tableName = new PdfPCell(new Phrase("Guest List "));
            tableName.Colspan = 3;
            tableName.HorizontalAlignment = Element.ALIGN_CENTER;
            tableName.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(tableName);

            PdfPCell header1 = new PdfPCell(new Phrase("Guest name"));
            header1.HorizontalAlignment = Element.ALIGN_CENTER;
            header1.VerticalAlignment = Element.ALIGN_MIDDLE;
            header1.FixedHeight = 40f;
            PdfPCell header2 = new PdfPCell(new Phrase("Guest attendance status"));
            header2.HorizontalAlignment = Element.ALIGN_CENTER;
            header2.VerticalAlignment = Element.ALIGN_MIDDLE;
            header2.FixedHeight = 40f;
            PdfPCell header3 = new PdfPCell(new Phrase("Guest joined at"));
            header3.HorizontalAlignment = Element.ALIGN_CENTER;
            header3.VerticalAlignment = Element.ALIGN_MIDDLE;
            header3.FixedHeight = 40f;

            table.AddCell(header1);
            table.AddCell(header2);
            table.AddCell(header3);

            PdfPCell temp;
            foreach (GuestTourAttendance guestTourAttendance in Tour.GuestAttendances)
            {
                temp = new PdfPCell(new Phrase(guestTourAttendance.TourReservation.Guest.Name + " " + guestTourAttendance.TourReservation.Guest.Surname));
                temp.HorizontalAlignment = Element.ALIGN_CENTER;
                temp.VerticalAlignment = Element.ALIGN_MIDDLE;
                temp.FixedHeight = 38f;
                table.AddCell(temp);
                temp = new PdfPCell(new Phrase(guestTourAttendance.Status.ToString()));
                temp.HorizontalAlignment = Element.ALIGN_CENTER;
                temp.VerticalAlignment = Element.ALIGN_MIDDLE;
                temp.FixedHeight = 38f;
                table.AddCell(temp);
                if (guestTourAttendance.KeyPointJoinedId != 0)
                {
                    temp = new PdfPCell(new Phrase(guestTourAttendance.KeyPointJoined.Title));
                }
                else
                {
                    temp = new PdfPCell(new Phrase("x"));
                }
                temp.HorizontalAlignment = Element.ALIGN_CENTER;
                temp.VerticalAlignment = Element.ALIGN_MIDDLE;
                temp.FixedHeight = 38f;
                table.AddCell(temp);
            }

            PdfPCell total = new PdfPCell(new Phrase("Total number of guests: "));
            total.Colspan = 2;
            total.HorizontalAlignment = Element.ALIGN_RIGHT;
            total.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(total);
            total = new PdfPCell(new Phrase(Tour.GuestAttendances.Count().ToString()));
            total.HorizontalAlignment = Element.ALIGN_LEFT;
            total.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(total);

            document.Add(table);

            document.Close();
        }

        private void LoadTour()
        {
            Tour = _tourService.GetTourInstance(Tour.Id);
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

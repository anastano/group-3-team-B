using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;

using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class PDFReportViewModel : INotifyPropertyChanged
    {
        private RenovationService _renovationService;
        public CreatePDFView CreatePDFView { get; set; }
        public PDFReportView PDFReportView { get; set; }
        public Owner Owner { get; set; }
        public DateRange DateRange { get; set; }
        public string DateRangeString { get; set; }
        public ObservableCollection<Renovation> Renovations { get; set; }

        #region OnPropertyChanged
        private DateTime _enteredStart;
        public DateTime EnteredStart
        {
            get => _enteredStart;
            set
            {
                if (value != _enteredStart)
                {
                    _enteredStart = value;
                    OnPropertyChanged(nameof(EnteredStart));
                }
            }
        }

        private DateTime _enteredEnd;
        public DateTime EnteredEnd
        {
            get => _enteredEnd;
            set
            {
                if (value != _enteredEnd)
                {
                    _enteredEnd = value;
                    OnPropertyChanged(nameof(EnteredEnd));
                }
            }
        }

        #endregion

        public RelayCommand GeneratePDFCommand { get; set; }
        public RelayCommand ClosePDFReportViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PDFReportViewModel(PDFReportView pDFReportView, CreatePDFView createPDFView, RenovationService renovationService, Owner owner, DateRange dateRange)
        {
            InitCommands();

            _renovationService = renovationService;

            CreatePDFView = createPDFView;
            PDFReportView = pDFReportView;

            DateRange = dateRange;
            DateRangeString = dateRange.Start.ToString("MM/dd/yyyy") + " - " + dateRange.End.ToString("MM/dd/yyyy");

            Owner = owner;
            Renovations = new ObservableCollection<Renovation>(_renovationService.GetInDateRangeByOwnerId(Owner.Id, dateRange));

        }

        #region Commands
        public void Executed_GeneratePDFCommand(object obj)
        {
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfFont font2 = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont font3 = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            PdfGraphics graphics = page.Graphics;

            graphics.DrawString("Report on renovations", font, PdfBrushes.Black, new PointF(170, 10)); ;
            graphics.DrawString("Owner: " + Owner.Name + " " + Owner.Surname, font2, PdfBrushes.Black, new PointF(10, 50));
            graphics.DrawString("Date range: " + DateRangeString, font2, PdfBrushes.Black, new PointF(10, 75));
            graphics.DrawString("The report includes renovations for all your accommodations in the specified date range.", font2, PdfBrushes.Black, new PointF(10, 110));

            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.Font = font3;

            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Start");
            dataTable.Columns.Add("End");
            dataTable.Columns.Add("Description");

            foreach (Renovation renovation in Renovations)
            {
                dataTable.Rows.Add(new object[] { renovation.Accommodation.Name, renovation.Start.ToString("MM/dd/yyyy"), renovation.End.ToString("MM/dd/yyyy"), renovation.Description});
            }
            pdfGrid.DataSource = dataTable;
            pdfGrid.AllowRowBreakAcrossPages = true;
            pdfGrid.Draw(page, new PointF(10, 140));

            String filename = Owner.Name + "_" + Owner.Surname + "_" +DateRange.Start.ToString("dd.MM.yyyy") + "-"+ DateRange.End.ToString("dd.MM.yyyy") + ".pdf";
            doc.Save(filename);
            doc.Close(true);
            PDFReportView.Close();
        }

        public bool CanExecute_GeneratePDFCommand(object obj)
        {
            return true;
        }
        public void Executed_ClosePDFReportViewCommand(object obj)
        {
            PDFReportView.Close();
        }

        public bool CanExecute_ClosePDFReportViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            GeneratePDFCommand = new RelayCommand(Executed_GeneratePDFCommand, CanExecute_GeneratePDFCommand);
            ClosePDFReportViewCommand = new RelayCommand(Executed_ClosePDFReportViewCommand, CanExecute_ClosePDFReportViewCommand);
        }
    }
}

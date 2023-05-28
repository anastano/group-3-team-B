using iTextSharp.text.pdf;
using iTextSharp.text;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class MyRatingsViewModel
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        private readonly RatingGivenByGuestService _guestRatingService;
        private int counter { get; set; }
        public Guest1 Guest { get; set; }
        public ObservableCollection<RatingGivenByOwner> RatingsGivenByOwners { get; set; }
        public List<KeyValuePair<int, int>> CleanlinessStatistics { get; set; }
        public List<KeyValuePair<int, int>> RuleComplianceStatistics { get; set; }
        public RelayCommand GeneratePdfCommand { get; set; }
        public MyRatingsViewModel(Guest1 guest)
        {
            _ownerRatingService = new RatingGivenByOwnerService();
            _guestRatingService = new RatingGivenByGuestService();
            Guest = guest;
            RatingsGivenByOwners = new ObservableCollection<RatingGivenByOwner>(_ownerRatingService.GetRatedByGuestId(_guestRatingService, Guest.Id));
            GeneratePdfCommand = new RelayCommand(ExecutedGeneratePdfCommand, CanExecute);
            counter = 0;
            LoadChartData();
        }
        public void ExecutedGeneratePdfCommand(object obj)
        {
            counter++;
            GenerateRatingReport();
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        private void LoadChartData()
        {
            CleanlinessStatistics = _ownerRatingService.GetRatingStatisticsForCategory(Guest.Id, "cleanliness");
            RuleComplianceStatistics = _ownerRatingService.GetRatingStatisticsForCategory(Guest.Id, "rulecompliance");
        }
        public void GenerateRatingReport()
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Guest.Name + "_" + Guest.Surname + "_" + counter.ToString() + "_report.pdf", FileMode.Create));
            document.Open();

            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Paragraph labelParagraph = new Paragraph("Report for Average Rating per Categories", labelFont);
            labelParagraph.Alignment = Element.ALIGN_CENTER;
            labelParagraph.SpacingAfter = 40f;
            document.Add(labelParagraph);

            // Add guest information
            Font infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Paragraph guestParagraph = new Paragraph($"Guest:", infoFont);
            guestParagraph.SpacingAfter = 5f;
            document.Add(guestParagraph);
            Font basicFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Paragraph guestInfoParagraph = new Paragraph(Guest.Name + " " + Guest.Surname, basicFont);
            guestInfoParagraph.SpacingAfter = 15f;
            document.Add(guestInfoParagraph);
            Paragraph datePar = new Paragraph($"Date:", infoFont);
            datePar.SpacingAfter = 5f;
            document.Add(datePar);
            Paragraph dateInfoPar = new Paragraph(DateTime.Now.ToString(), basicFont);
            dateInfoPar.SpacingAfter = 15f;
            document.Add(dateInfoPar);
            // Add category and average rating information
            Paragraph category1Paragraph = new Paragraph($"Category: Rule Compliance", infoFont);
            category1Paragraph.SpacingAfter = 5f;
            document.Add(category1Paragraph);
            double avgRatingRuleCompliance = _ownerRatingService.GetAverageRatingForRuleCompliance(Guest.Id);
            Paragraph rating1Paragraph = new Paragraph($"Average Rating: {avgRatingRuleCompliance.ToString("0.00")}", basicFont);
            rating1Paragraph.SpacingAfter = 15f;
            document.Add(rating1Paragraph);
            double avgRatingCleanliness = _ownerRatingService.GetAverageRatingForCleanliness(Guest.Id);
            Paragraph category2Paragraph = new Paragraph($"Category: Cleanliness", infoFont);
            category2Paragraph.SpacingAfter = 5f;
            document.Add(category2Paragraph);
            Paragraph rating2Paragraph = new Paragraph($"Average Rating: {avgRatingCleanliness.ToString("0.00")}", basicFont);
            rating2Paragraph.SpacingAfter = 5f;
            document.Add(rating2Paragraph);

            document.Close();

            string browserPath = GetDefaultWebBrowserPath();

            // Open the PDF file with the default web browser
            if (!string.IsNullOrEmpty(browserPath))
            {
                Process.Start(new ProcessStartInfo(browserPath, $"file:///{Path.GetFullPath(Guest.Name + "_" + Guest.Surname + "_" + counter.ToString() + "_report.pdf")}")
                {
                    UseShellExecute = true
                });
            }
        }
        private string GetDefaultWebBrowserPath()
        {
            // Default web browser registry key for Windows
            const string registryKey = @"HTTP\shell\open\command";

            using (Microsoft.Win32.RegistryKey browserKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(registryKey))
            {
                if (browserKey != null)
                {
                    string path = browserKey.GetValue(null) as string;
                    if (!string.IsNullOrEmpty(path))
                    {
                        // Extract the path to the browser executable
                        path = path.Replace("\"", "");
                        if (path.Contains(".exe"))
                        {
                            path = path.Substring(0, path.IndexOf(".exe") + 4);
                        }

                        return path;
                    }
                }
            }
            return string.Empty;
        }
    }
}

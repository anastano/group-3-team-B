//using ceTe.DynamicPDF;
//using ceTe.DynamicPDF.PageElements;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Guest1 = SIMS_HCI_Project.Domain.Models.Guest1;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RatingGivenByOwnerService
    {
        private readonly IRatingGivenByOwnerRepository _ratingRepository;

        public RatingGivenByOwnerService()
        {
            _ratingRepository = Injector.Injector.CreateInstance<IRatingGivenByOwnerRepository>();
        }

        public RatingGivenByOwner GetById(int id)
        {
            return _ratingRepository.GetById(id);
        }

        public RatingGivenByOwner GetByReservationId(int reservationId)
        {
            return _ratingRepository.GetByReservationId(reservationId);
        }
        public List<RatingGivenByOwner> GetByGuestId(int guestId)
        {
            return _ratingRepository.GetByGuestId(guestId);
        }
        public List<RatingGivenByOwner> GetAll()
        {
            return _ratingRepository.GetAll();
        }
        public double GetGuestAverageRate(Guest1 guest)
        {
            int ratingsSum = 0;
            int counter = 0;

            foreach (RatingGivenByOwner rating in GetByGuestId(guest.Id))
            {
                ratingsSum += rating.Cleanliness + rating.RuleCompliance;
                counter += 2;
            }
            return counter == 0 ? 0 : (double)ratingsSum / counter;
        }
        public List<AccommodationReservation> GetUnratedReservations(int ownerId, AccommodationReservationService reservationService)
        {
            List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();
            DateRange possibleDateRange = new DateRange(DateTime.Today.AddDays(-5), DateTime.Today);

            foreach (AccommodationReservation reservation in reservationService.GetByOwnerId(ownerId))
            {
                DateRange reservationDateRange = new DateRange(reservation.Start, reservation.End);
                if (reservationDateRange.IsEndInside(possibleDateRange) && !IsReservationRated(reservation))
                {
                    unratedReservations.Add(reservation);
                }
            }
            return unratedReservations;
        }
        public List<RatingGivenByOwner> GetRatedByGuestId(RatingGivenByGuestService guestRatingService, int guestId)
        {
            List<RatingGivenByOwner> ratedByOwnerId = new List<RatingGivenByOwner>();

            foreach (RatingGivenByOwner rating in GetByGuestId(guestId))
            {
                if (guestRatingService.GetByReservationId(rating.ReservationId) != null)
                {
                    ratedByOwnerId.Add(rating);
                }
            }
            return ratedByOwnerId;
        }

        public bool IsReservationRated(AccommodationReservation reservation)
        {
            return (GetByReservationId(reservation.Id) != null) ? true : false;
        }

        public void Add(RatingGivenByOwner rating)
        {
            _ratingRepository.Add(rating);
        }
        public List<KeyValuePair<int, int>> GetRatingStatisticsForCategory(int guestId, string categoryName)
        {
            List<KeyValuePair<int, int>> statstics = new List<KeyValuePair<int, int>>();
            for (int i = 1; i <= 5; i++)
            {
                statstics.Add(new KeyValuePair<int, int>(i, _ratingRepository.GetRatingCountForCategory(guestId, categoryName, i)));
            }
            return statstics;
        }
        public void GenerateRatingReport(Guest1 guest)
        {
            Document document = new Document();

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(guest.Name + "_" + guest.Surname + "_report.pdf", FileMode.Create));
            document.Open();

            // Add report label
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Paragraph labelParagraph = new Paragraph("Report for Average Rating per Categories", labelFont);
            labelParagraph.Alignment = Element.ALIGN_CENTER;
            labelParagraph.SpacingAfter = 40f;
            document.Add(labelParagraph);

            // Add guest information
            Font infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Paragraph guestParagraph =    new Paragraph($"Guest:", infoFont);
            guestParagraph.SpacingAfter = 5f;
            document.Add(guestParagraph);
            Font ginfoFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Paragraph guestInfoParagraph = new Paragraph(guest.Name + " " +  guest.Surname, ginfoFont);
            guestInfoParagraph.SpacingAfter = 5f;
            document.Add(guestInfoParagraph);
            // Add category and average rating information
            Font categoryFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Font ratingFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Paragraph category1Paragraph = new Paragraph($"Category: Rule Compliance", categoryFont);
            category1Paragraph.SpacingAfter = 5f;
            document.Add(category1Paragraph);
            double avgRatingRuleCompliance = _ratingRepository.GetAverageRatingForRuleCompliance(guest.Id);
            Paragraph rating1Paragraph = new Paragraph($"Average Rating: {avgRatingRuleCompliance.ToString("0.00")}", ratingFont);
            rating1Paragraph.SpacingAfter = 5f;
            document.Add(rating1Paragraph);
            double avgRatingCleanliness = _ratingRepository.GetAverageRatingForCleanliness(guest.Id);
            Paragraph category2Paragraph = new Paragraph($"Category: Cleanliness", categoryFont);
            category2Paragraph.SpacingAfter = 5f;
            document.Add(category2Paragraph);
            Paragraph rating2Paragraph = new Paragraph($"Average Rating: {avgRatingCleanliness.ToString("0.00")}", ratingFont);
            rating2Paragraph.SpacingAfter = 5f;
            document.Add(rating2Paragraph);

            document.Close();

            string browserPath = GetDefaultWebBrowserPath();

            // Open the PDF file with the default web browser
            if (!string.IsNullOrEmpty(browserPath))
            {
                Process.Start(new ProcessStartInfo(browserPath, $"file:///{Path.GetFullPath(guest.Name + "_" + guest.Surname + "_report.pdf")}")
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

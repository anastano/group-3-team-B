using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum TourRequestStatus { PENDING, ACCEPTED, INVALID }; // used for both regular and complex tours

    public class RegularTourRequest : TourRequest
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Language { get; set; }
        public int GuestNumber { get; set; }
        public string Description { get; set; }
        public DateRange DateRange { get; set; }
        public DateTime SubmittingDate { get; set; } //SubmissionDate rename pls!!! 
        public int ComplexTourRequestId { get; set; } 
        public int TourId { get; set; } // u koju turu se konvertovalo, jer cOmPleXsnE
        public Tour Tour { get; set; }
        public ComplexTourRequest ComplexTourRequest { get; set; } // not sure if needed

        public bool IsPartOfComplex { get => ComplexTourRequestId > 0; }

        public RegularTourRequest()
        {
            DateRange = new DateRange();
        }

        public RegularTourRequest(int guestId, Guest2 guest, Location location, string language, int guestNumber, string description, DateRange dateRange, int complexTourRequestId)
        {
            GuestId = guestId;
            Guest = guest;
            Location.City = city;
            Location.Country = country;
            Language = language;
            GuestNumber = guestNumber;
            Description = description;
            DateRange = dateRange;
            ComplexTourRequestId = complexTourRequestId;

            SubmittingDate = DateTime.Now;
            Status = TourRequestStatus.PENDING;
        }

        public RegularTourRequest(RegularTourRequest source)
        {
            Id = source.Id;
            GuestId = source.GuestId;
            Guest = source.Guest;
            LocationId = source.LocationId;
            Location = source.Location;
            Language = source.Language;
            GuestNumber = source.GuestNumber;
            Description = source.Description;
            DateRange = source.DateRange;
            SubmittingDate = source.SubmittingDate;
            ComplexTourRequestId = source.ComplexTourRequestId;
            TourId = source.TourId;
            Tour = source.Tour;
            ComplexTourRequest = source.ComplexTourRequest;
        }

        public void AssignTour(Tour tour)
        {
            this.Tour = tour;
            this.TourId = tour.Id;
        }
    }
}

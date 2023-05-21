using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;

namespace SIMS_HCI_Project.Domain.Models
{
     public enum RegularRequestStatus { PENDING, ACCEPTED, INVALID };

    public class RegularTourRequest
    {
        public int Id { get; set; }
        public RegularRequestStatus Status { get; set; }
        public int GuestId { get; set; }
        public Guest2 Guest { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Language { get; set; }
        public int GuestNumber { get; set; }
        public string Description { get; set; }
        public DateRange DateRange { get; set; }
        public DateTime SubmittingDate { get; set; } //SubmissionDate rename pls!!! 
        public bool IsPartOfComplex { get; set; } 


        public RegularTourRequest()
        {
            DateRange = new DateRange();
        }

        public RegularTourRequest(int guestId, Guest2 guest, Location location, string language, int guestNumber, string description, DateRange dateRange, bool isPartOfComplex)
        {
            GuestId = guestId;
            Guest = guest;
            Location = location;
            Language = language;
            GuestNumber = guestNumber;
            Description = description;
            DateRange = dateRange;
            IsPartOfComplex = isPartOfComplex;

            SubmittingDate = DateTime.Now;
            Status = RegularRequestStatus.PENDING;
        }
    }
}

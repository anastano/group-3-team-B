using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class TourDTO : IDataErrorInfo, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guide Guide { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if(string.IsNullOrEmpty(_title) && Touched["Title"] == false)
                {
                    Touched["Title"] = true;
                }

            }
        }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public List<int> KeyPointsIds { get; set; }
        public List<TourKeyPoint> KeyPoints { get; set; }
        public List<TourTime> DepartureTimes { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; }

        public Dictionary<string, bool> Touched = new Dictionary<string, bool>
        {
           {"Title", false}
        };

        public TourDTO()
        {

        }

        public TourDTO(Tour tour)
        {
            Id = tour.Id;
            GuideId = tour.GuideId;
            Guide = tour.Guide;
            Title = tour.Title;
            LocationId = tour.LocationId;
            Location = tour.Location;
            Description = tour.Description;
            Language = tour.Language;
            MaxGuests = tour.MaxGuests;
            KeyPointsIds = tour.KeyPointsIds;
            KeyPoints = tour.KeyPoints;
            DepartureTimes = tour.DepartureTimes;
            Duration = tour.Duration;
            Images = tour.Images;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title) && Touched["Title"] == true)
                            return "Title is required";
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                            return "Description is required";
                        break;
                    case "Location":
                        if (Location == null || string.IsNullOrEmpty(Location.City) || string.IsNullOrEmpty(Location.Country))
                            return "Location is required";
                        break;
                    case "Language":
                        if (string.IsNullOrEmpty(Language))
                            return "Language is required";
                        break;
                    case "MaxGuests":
                        if (MaxGuests < 1)
                            return "MaxGuests is required and cannot be less than 1";
                        break;
                    case "Duration":
                        if (Duration < 1)
                            return "Duration is required and cannot be less than 1h";
                        break;
                    case "DepartureTimes":
                        if (DepartureTimes.Count < 1)
                            return "Tour must have at least one Departure Time";
                        break;
                    case "KeyPoints":
                        if (KeyPoints.Count < 2)
                            return "Tour must have at least two Key Points";
                        break;
                    case "Images":
                        if (Images.Count > 5)
                            return "You can upload max of 5 images";
                        break;
                }
                return null;
            }

        }

        private readonly string[] _validatedProperties = { "Title", "Description", "Language", "Location", "MaxGuests", "Duration", "DepartureTimes", "KeyPoints", "Images" };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public Tour ToTour()
        {
            Tour tour = new Tour();

            tour.Id = Id;
            tour.GuideId = GuideId;
            tour.Guide = Guide;
            tour.Title = Title;
            tour.LocationId = LocationId;
            tour.Location = Location;
            tour.Description = Description;
            tour.Language = Language;
            tour.MaxGuests = MaxGuests;
            tour.KeyPointsIds = KeyPointsIds;
            tour.KeyPoints = KeyPoints;
            tour.DepartureTimes = DepartureTimes;
            tour.Duration = Duration;
            tour.Images = Images;

            return tour;
        }
    }
}

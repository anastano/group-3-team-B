using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Tour : ISerializable, IDataErrorInfo, INotifyPropertyChanged, INotifyCollectionChanged 
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guide Guide { get; set; }
        public string Title { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public List<int> KeyPointsIds { get; set; }
        public List<TourKeyPoint> KeyPoints { get; set; }
        public List<TourTime> DepartureTimes { get; set; }
        public int Duration { get; set; }
       
        public List<string> Images { get; set; } // [Maybe] TODO: Change to URI type

        public Tour() {

            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
        }

        public Tour(Guide guide)
        {
            Guide = guide;
            GuideId = guide.Id;


            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
        }

        public Tour(string title, int locationId, string description, string language, int maxGuestNumber, int duration)
        {
            Title = title;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuestNumber;
            Duration = duration;
            //Available = maxGuestNumber;

            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
            KeyPoints = new List<TourKeyPoint>();
        }

        public string[] ToCSV()     //anastaNOTE: add Available in csv or not?
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), Title, LocationId.ToString(), Description, Language, MaxGuests.ToString(), string.Join(",", KeyPointsIds), Duration.ToString(), string.Join(",", Images)};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            Title = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Description = values[4];
            Language = values[5];
            MaxGuests = Convert.ToInt32(values[6]);
            KeyPointsIds = new List<int>(Array.ConvertAll(values[7].Split(","), Convert.ToInt32));
            Duration = Convert.ToInt32(values[8]);
            Images = new List<string>(values[9].Split(","));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                            return "Title is required";
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                            return "Description is required";
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
                }
                return null;
            }

        }

        private readonly string[] _validatedProperties = { "Title", "Description", "Language", "MaxGuests", "Duration", "DepartureTimes", "KeyPoints" };

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
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

    }
}

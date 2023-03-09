using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string GuideId { get; set; }
        public Guide Guide { get; set; }
        public string Title { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuestNumber { get; set; }
        public List<int> KeyPointsIds { get; set; }
        public List<TourKeyPoint> KeyPoints { get; set; }
        public List<TourTime> DepartureTimes { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; } // [Maybe] TODO: Change to URI type

        public Tour() { }

        public Tour(Guide guide)
        {
            Guide = guide;
            GuideId = guide.Id;
        }

        public Tour(string title, int locationId, string description, string language, int maxGuestNumber, int duration)
        {
            Title = title;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuestNumber = maxGuestNumber;
            Duration = duration;

            Location = new Location();
            Images = new List<string>();
            DepartureTimes = new List<TourTime>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId, Title, LocationId.ToString(), Description, Language, MaxGuestNumber.ToString(), string.Join(",", KeyPointsIds), Duration.ToString(), string.Join(",", Images)};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = values[1];
            Title = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Description = values[4];
            Language = values[5];
            MaxGuestNumber = Convert.ToInt32(values[6]);
            KeyPointsIds = new List<int>(Array.ConvertAll(values[7].Split(","), Convert.ToInt32));
            Duration = Convert.ToInt32(values[8]);
            Images = new List<string>(values[9].Split(","));
        }
    }
}

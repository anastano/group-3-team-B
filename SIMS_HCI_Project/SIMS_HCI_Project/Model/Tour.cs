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
        public string Title { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuestNumber { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; } // [Maybe] TODO: Change to URI type

        public Tour() { }

        public Tour(string title, int locationId, string description, string language, int maxGuestNumber, DateTime startDate, int duration, List<string> images)
        {
            Title = title;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuestNumber = maxGuestNumber;
            StartDate = startDate;
            Duration = duration;
            Images = images;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, LocationId.ToString(), Description, Language, MaxGuestNumber.ToString(), StartDate.ToString(), Duration.ToString(), string.Join(",", Images)};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuestNumber = Convert.ToInt32(values[5]);
            StartDate = Convert.ToDateTime(values[6]);
            Duration = Convert.ToInt32(values[7]);
            Images = new List<string>(values[8].Split(","));
        }
    }
}

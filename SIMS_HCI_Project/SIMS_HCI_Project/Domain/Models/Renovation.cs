using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Renovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }

        public Renovation() { }

        public Renovation(int accommodationId, DateTime start, DateTime end, string description) 
        { 
            AccommodationId = accommodationId;
            Start= start;
            End= end;
            Description= description;
        }

        public Renovation(Renovation renovation)
        {
            AccommodationId = renovation.AccommodationId;
            Start= renovation.Start;
            End= renovation.End;
            Description= renovation.Description;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                Start.ToString("MM/dd/yyyy"),
                End.ToString("MM/dd/yyyy"),
                Description

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            Start = DateTime.ParseExact(values[2], "MM/dd/yyyy", null);
            End = DateTime.ParseExact(values[3], "MM/dd/yyyy", null);
            Description = values[4];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Renovation
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }

        public Renovation() { }

        public Renovation(int accommodationId, DateTime start, DateTime end) 
        { 
            AccommodationId = accommodationId;
            Start= start;
            End= end;
            Description= "";
        }

        public Renovation(Renovation renovation)
        {
            AccommodationId = renovation.AccommodationId;
            Start= renovation.Start;
            End= renovation.End;
            Description= renovation.Description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Tour
    {
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public List<DateTime> DepartureTimes { get; set; }
        public List<DateTime> CompletionTimes { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; } // [Maybe] Change to URI type



    }
}

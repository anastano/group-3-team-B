using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class TourKeyPoint : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public Location Location { get; set; } // [Maybe] TODO: Each Key Point should belong to one location? Make a dropdown + input?

        public TourKeyPoint() { }

        public TourKeyPoint(string title)
        {
            Title = title;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
        }
    }
}

using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourKeyPoint
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public TourKeyPoint() { }

        public TourKeyPoint(string title)
        {
            Title = title;
        }

        public override string? ToString()
        {
            return Title;
        }
    }
}

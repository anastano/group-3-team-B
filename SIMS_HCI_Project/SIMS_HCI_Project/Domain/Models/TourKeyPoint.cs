using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class TourKeyPoint : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public TourKeyPoint() { }

        public TourKeyPoint(string title)
        {
            Title = title;
        }

        public override string? ToString() // 0 refesrnces, delete?
        {
            return Title;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
        }
    }
}

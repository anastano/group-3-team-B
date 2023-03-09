using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public enum TourKeyPointStatus { NOT_STARTED, IN_PROGRESS, COMPLETED};

    public class TourKeyPoint : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TourKeyPointStatus Status { get; set; }

        public TourKeyPoint() { }

        public TourKeyPoint(string title)
        {
            Title = title;
            Status = TourKeyPointStatus.NOT_STARTED;
        }

        public override string? ToString()
        {
            return Title;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            Enum.TryParse(values[2], out TourKeyPointStatus Status);
        }
    }
}

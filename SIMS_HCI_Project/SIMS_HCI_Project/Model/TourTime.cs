using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public enum TourStatus { NOT_STARTED, IN_PROGRESS, COMPLETED, CANCELED };

    public class TourTime : ISerializable
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DepartureTime { get; set; }
        public TourStatus Status { get; set; }

        public TourTime() { }

        public TourTime(int tourId, DateTime departureTime)
        {
            TourId = tourId;
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), DepartureTime.ToString(), Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            DepartureTime = Convert.ToDateTime(values[1]);
            Enum.TryParse(values[2], out TourStatus Status);
        }
    }
}

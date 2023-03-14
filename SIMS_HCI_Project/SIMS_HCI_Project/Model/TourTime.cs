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
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DepartureTime { get; set; }
        public TourStatus Status { get; set; }
        public TourKeyPoint CurrentKeyPoint { get; set; }
        public int CurrentKeyPointId { get; set; }

        public TourTime() { }

        public TourTime(DateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;
        }

        public TourTime(int tourId, DateTime departureTime)
        {
            TourId = tourId;
            DepartureTime = departureTime;
            Status = TourStatus.NOT_STARTED;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), DepartureTime.ToString(), Status.ToString(), CurrentKeyPointId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            DepartureTime = Convert.ToDateTime(values[2]);
            Enum.TryParse(values[3], out TourStatus Status);
            CurrentKeyPointId = Convert.ToInt32(values[4]);
        }
    }
}

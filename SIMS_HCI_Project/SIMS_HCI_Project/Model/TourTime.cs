using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class TourTime : ISerializable
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DepartureTime { get; set; }

        public TourTime() { }

        public TourTime(int tourId, DateTime departureTime)
        {
            TourId = tourId;
            DepartureTime = departureTime;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), DepartureTime.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            DepartureTime = Convert.ToDateTime(values[1]);
        }
    }
}

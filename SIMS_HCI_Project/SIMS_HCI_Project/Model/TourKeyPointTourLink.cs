using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class TourKeyPointTourLink : ISerializable
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int TourKeyPointId { get; set; }
        public TourKeyPoint TourKeyPoint { get; set; }

        public TourKeyPointTourLink() { }

        public TourKeyPointTourLink(int tourId, int tourKeyPointId)
        {
            TourId = tourId;
            TourKeyPointId = tourKeyPointId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), TourKeyPointId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            TourKeyPointId = Convert.ToInt32(values[1]);
        }
    }
}

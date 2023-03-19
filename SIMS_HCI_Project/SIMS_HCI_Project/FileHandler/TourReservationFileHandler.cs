using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    internal class TourReservationFileHandler
    {
        private const string path = "../../../Resources/Database/tourReservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        public TourReservationFileHandler()
        {
            _serializer = new Serializer<TourReservation>();
        }

        public List<TourReservation> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<TourReservation> reservations)
        {
            _serializer.ToCSV(path, reservations);
        }
    }
}

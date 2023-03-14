using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class AccommodationReservationFileHandler
    {
        private const string path = "../../../Resources/Database/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;


        public AccommodationReservationFileHandler()
        {
            _serializer = new Serializer<AccommodationReservation>();
        }

        public List<AccommodationReservation> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<AccommodationReservation> reservations)
        {
            _serializer.ToCSV(path, reservations);
        }
    }
}

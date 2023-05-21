using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class AccommodationReservationFileHandler
    {
        private const string path = "../../../Resources/Database/accommodationReservations.csv";
        private const char Delimiter = '|';

        public AccommodationReservationFileHandler() {}
        public List<AccommodationReservation> Load()
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();

            foreach (string line in File.ReadLines(path))
            {
                string[] csvValues = line.Split(Delimiter);
                AccommodationReservation reservation = new AccommodationReservation();

                reservation.Id = int.Parse(csvValues[0]);
                reservation.AccommodationId = int.Parse(csvValues[1]);
                reservation.GuestId = int.Parse(csvValues[2]);
                reservation.Start = DateTime.ParseExact(csvValues[3], "MM/dd/yyyy", null);
                reservation.End = DateTime.ParseExact(csvValues[4], "MM/dd/yyyy", null);
                reservation.GuestNumber = int.Parse(csvValues[5]);
                Enum.TryParse(csvValues[6], out AccommodationReservationStatus status);
                reservation.Status = status;

                reservations.Add(reservation);
            }

            return reservations;
        }
        public void Save(List<AccommodationReservation> reservations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (AccommodationReservation reservation in reservations)
            {
                string[] csvValues =
                {
                    reservation.Id.ToString(),
                    reservation.AccommodationId.ToString(),
                    reservation.GuestId.ToString(),
                    reservation.Start.ToString("MM/dd/yyyy"),
                    reservation.End.ToString("MM/dd/yyyy"),
                    reservation.GuestNumber.ToString(),
                    reservation.Status.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}

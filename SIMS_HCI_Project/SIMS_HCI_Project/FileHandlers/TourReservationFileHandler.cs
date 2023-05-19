using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    internal class TourReservationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourReservations.csv";
        private const char Delimiter = '|';

        public TourReservationFileHandler() { }

        public List<TourReservation> Load()
        {
            List<TourReservation> reservations = new List<TourReservation>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                TourReservation reservation = new TourReservation();

                reservation.Id = Convert.ToInt32(csvValues[0]);
                reservation.TourTimeId = Convert.ToInt32(csvValues[1]);
                reservation.GuestId = Convert.ToInt32(csvValues[2]);
                reservation.PartySize = Convert.ToInt32(csvValues[3]);
                Enum.TryParse(csvValues[4], out TourReservationStatus status);
                reservation.Status = status;
                reservation.VoucherUsedId = Convert.ToInt32(csvValues[5]);

                reservations.Add(reservation);
            }

            return reservations;
        }

        public void Save(List<TourReservation> reservations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (TourReservation reservation in reservations)
            {
                string[] csvValues =
                {
                    reservation.Id.ToString(),
                    reservation.TourTimeId.ToString(),
                    reservation.GuestId.ToString(),
                    reservation.PartySize.ToString(),
                    reservation.Status.ToString(),
                    reservation.VoucherUsedId.ToString() 
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

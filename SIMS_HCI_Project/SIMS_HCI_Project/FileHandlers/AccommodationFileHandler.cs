using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.FileHandlers
{
    public class AccommodationFileHandler
    {
        private const string path = "../../../Resources/Database/accommodations.csv";
        private const char Delimiter = '|';


        public AccommodationFileHandler() {}

        public List<Accommodation> Load()
        {
            List<Accommodation> accommodations = new List<Accommodation>();

            foreach (string line in File.ReadLines(path))
            {
                string[] csvValues = line.Split(Delimiter);
                Accommodation accommodation = new Accommodation();

                accommodation.Id = int.Parse(csvValues[0]);
                accommodation.OwnerId = Convert.ToInt32(csvValues[1]);
                accommodation.Name = csvValues[2];
                accommodation.LocationId = int.Parse(csvValues[3]);
                Enum.TryParse(csvValues[4], out AccommodationType type);
                accommodation.Type = type;
                accommodation.MaxGuests = int.Parse(csvValues[5]);
                accommodation.MinimumReservationDays = int.Parse(csvValues[6]);
                accommodation.CancellationDeadlineInDays = int.Parse(csvValues[7]);
                accommodation.Images = new List<string>(csvValues[8].Split(","));
                accommodation.FirstImage = accommodation.Images.FirstOrDefault();

                accommodations.Add(accommodation);
            }

            return accommodations;
        }

        public void Save(List<Accommodation> accommodations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Accommodation accommodation in accommodations)
            {
                string[] csvValues =
                {
                    accommodation.Id.ToString(),
                    accommodation.OwnerId.ToString(),
                    accommodation.Name,
                    accommodation.LocationId.ToString(),
                    accommodation.Type.ToString(),
                    accommodation.MaxGuests.ToString(),
                    accommodation.MinimumReservationDays.ToString(),
                    accommodation.CancellationDeadlineInDays.ToString(),
                    string.Join(",", accommodation.Images)
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}

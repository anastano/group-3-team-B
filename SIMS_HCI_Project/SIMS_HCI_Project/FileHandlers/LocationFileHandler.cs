using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class LocationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/locations.csv";
        private const char Delimiter = '|';

        public LocationFileHandler() { }

        public List<Location> Load()
        {
            List<Location> locations = new List<Location>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                Location location = new Location();

                location.Id = Convert.ToInt32(csvValues[0]);
                location.City = csvValues[1];
                location.Country = csvValues[2];

                locations.Add(location);
            }

            return locations;
        }

        public void Save(List<Location> locations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Location location in locations)
            {
                string[] csvValues =
                {
                    location.Id.ToString(),
                    location.City,
                    location.Country
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

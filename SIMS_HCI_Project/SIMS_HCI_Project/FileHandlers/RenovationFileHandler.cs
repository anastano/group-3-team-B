using SIMS_HCI_Project.Domain.Models;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RenovationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/renovations.csv";
        private const char Delimiter = '|';

        public RenovationFileHandler()
        {

        }

        public List<Renovation> Load()
        {
            List<Renovation> renovations = new List<Renovation>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                Renovation renovation = new Renovation();

                renovation.Id = int.Parse(csvValues[0]);
                renovation.AccommodationId = int.Parse(csvValues[1]);
                renovation.Start = DateTime.ParseExact(csvValues[2], "MM/dd/yyyy", null);
                renovation.End = DateTime.ParseExact(csvValues[3], "MM/dd/yyyy", null);
                renovation.Description = csvValues[4];

                renovations.Add(renovation);
            }

            return renovations;
        }

        public void Save(List<Renovation> renovations)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Renovation renovation in renovations)
            {
                string[] csvValues =
                {
                    renovation.Id.ToString(),
                    renovation.AccommodationId.ToString(),
                    renovation.Start.ToString("MM/dd/yyyy"),
                    renovation.End.ToString("MM/dd/yyyy"),
                    renovation.Description

                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

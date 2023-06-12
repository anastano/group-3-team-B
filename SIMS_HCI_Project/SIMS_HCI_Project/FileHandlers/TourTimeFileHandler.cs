using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourTimeFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourTimes.csv";
        private const char Delimiter = '|';

        public TourTimeFileHandler()
        {
        }

        public List<TourTime> Load()
        {
            List<TourTime> tourTimes = new List<TourTime>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                TourTime tourTime = new TourTime();

                tourTime.Id = Convert.ToInt32(csvValues[0]);
                tourTime.TourId = Convert.ToInt32(csvValues[1]);
                tourTime.DepartureTime = DateTime.ParseExact(csvValues[2], "M/d/yyyy h:mm:ss tt", null);
                Enum.TryParse(csvValues[3], out TourStatus status);
                tourTime.Status = status;
                tourTime.CurrentKeyPointIndex = Convert.ToInt32(csvValues[4]);

                tourTimes.Add(tourTime);
            }

            return tourTimes;
        }

        public void Save(List<TourTime> tourTimes)
        {
            StringBuilder csv = new StringBuilder();

            foreach (TourTime tourTime in tourTimes)
            {
                string[] csvValues =
                {
                    tourTime.Id.ToString(),
                    tourTime.TourId.ToString(),
                    tourTime.DepartureTime.ToString("M/d/yyyy h:mm:ss tt"),
                    tourTime.Status.ToString(),
                    tourTime.CurrentKeyPointIndex.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

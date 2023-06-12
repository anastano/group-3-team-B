using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourKeyPointFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourkeypoints.csv";
        private const char Delimiter = '|';

        public TourKeyPointFileHandler() { }

        public List<TourKeyPoint> Load()
        {
            List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                TourKeyPoint keyPoint = new TourKeyPoint();

                keyPoint.Id = Convert.ToInt32(csvValues[0]);
                keyPoint.Title = csvValues[1];

                keyPoints.Add(keyPoint);
            }

            return keyPoints;
        }

        public void Save(List<TourKeyPoint> tourKeyPoints)
        {
            StringBuilder csv = new StringBuilder();

            foreach (TourKeyPoint tourKeyPoint in tourKeyPoints)
            {
                string[] csvValues =
                {
                    tourKeyPoint.Id.ToString(),
                    tourKeyPoint.Title
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

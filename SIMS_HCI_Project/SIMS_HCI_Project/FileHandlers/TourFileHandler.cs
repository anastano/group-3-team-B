using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tours.csv";
        private const char Delimiter = '|';

        public TourFileHandler()
        {

        }

        public List<Tour> Load()
        {
            List<Tour> tours = new List<Tour>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                Tour tour = new Tour();

                tour.Id = Convert.ToInt32(csvValues[0]);
                tour.GuideId = Convert.ToInt32(csvValues[1]);
                tour.Title = csvValues[2];
                tour.LocationId = Convert.ToInt32(csvValues[3]);
                tour.Description = csvValues[4];
                tour.Language = csvValues[5];
                tour.MaxGuests = Convert.ToInt32(csvValues[6]);
                tour.KeyPointsIds = new List<int>(Array.ConvertAll(csvValues[7].Split(","), Convert.ToInt32));
                tour.Duration = Convert.ToInt32(csvValues[8]);
                tour.Images = new List<string>(csvValues[9].Split(","));

                tours.Add(tour);
            }

            return tours;
        }

        public void Save(List<Tour> tours)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Tour tour in tours)
            {
                string[] csvValues = 
                { 
                    tour.Id.ToString(),
                    tour.GuideId.ToString(),
                    tour.Title,
                    tour.LocationId.ToString(),
                    tour.Description,
                    tour.Language,
                    tour.MaxGuests.ToString(),
                    string.Join(",", tour.KeyPointsIds),
                    tour.Duration.ToString(), 
                    string.Join(",", tour.Images) 
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

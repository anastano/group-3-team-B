using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourRatingFileHandler
    {
        private const string path = "../../../Resources/Database/tourRatings.csv";

        private readonly Serializer<TourRating> _serializer;


        public TourRatingFileHandler()
        {
            _serializer = new Serializer<TourRating>();
        }

        public List<TourRating> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<TourRating> ratings)
        {
            _serializer.ToCSV(path, ratings);
        }
    }
}

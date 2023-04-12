using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    internal class RatingGivenByGuestFileHandler
    {
        private const string path = "../../../Resources/Database/ratingsGivenByGuest.csv";

        private readonly Serializer<RatingGivenByGuest> _serializer;


        public RatingGivenByGuestFileHandler()
        {
            _serializer = new Serializer<RatingGivenByGuest>();
        }

        public List<RatingGivenByGuest> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<RatingGivenByGuest> ratings)
        {
            _serializer.ToCSV(path, ratings);
        }
    }
}

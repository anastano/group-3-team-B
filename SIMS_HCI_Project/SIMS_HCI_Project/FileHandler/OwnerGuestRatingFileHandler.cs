using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class OwnerGuestRatingFileHandler
    {
        private const string path = "../../../Resources/Database/ownerGuestRatings.csv";

        private readonly Serializer<OwnerGuestRating> _serializer;


        public OwnerGuestRatingFileHandler()
        {
            _serializer = new Serializer<OwnerGuestRating>();
        }

        public List<OwnerGuestRating> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<OwnerGuestRating> ratings)
        {
            _serializer.ToCSV(path, ratings);
        }

    }
}

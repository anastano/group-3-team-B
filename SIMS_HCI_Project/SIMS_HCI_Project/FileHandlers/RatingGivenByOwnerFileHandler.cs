using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RatingGivenByOwnerFileHandler
    {
        private const string path = "../../../Resources/Database/ratingsGivenByOwner.csv";

        private readonly Serializer<RatingGivenByOwner> _serializer;


        public RatingGivenByOwnerFileHandler()
        {
            _serializer = new Serializer<RatingGivenByOwner>();
        }

        public List<RatingGivenByOwner> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<RatingGivenByOwner> ratings)
        {
            _serializer.ToCSV(path, ratings);
        }
    }
}

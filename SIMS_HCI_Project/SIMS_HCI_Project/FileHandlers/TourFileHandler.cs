using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tours.csv";

        private readonly Serializer<Tour> _serializer;

        public TourFileHandler()
        {
            _serializer = new Serializer<Tour>();
        }

        public List<Tour> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<Tour> tours)
        {
            _serializer.ToCSV(FilePath, tours);
        }
    }
}

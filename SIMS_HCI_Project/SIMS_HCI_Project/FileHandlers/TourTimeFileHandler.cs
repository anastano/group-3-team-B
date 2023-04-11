using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourTimeFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourTimes.csv";

        private readonly Serializer<TourTime> _serializer;

        public TourTimeFileHandler()
        {
            _serializer = new Serializer<TourTime>();
        }

        public List<TourTime> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<TourTime> tourTimes)
        {
            _serializer.ToCSV(FilePath, tourTimes);
        }
    }
}

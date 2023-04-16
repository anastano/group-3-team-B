using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourKeyPointFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourkeypoints.csv";

        private readonly Serializer<TourKeyPoint> _serializer;

        public TourKeyPointFileHandler()
        {
            _serializer = new Serializer<TourKeyPoint>();
        }

        public List<TourKeyPoint> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<TourKeyPoint> tourKeyPoints)
        {
            _serializer.ToCSV(FilePath, tourKeyPoints);
        }
    }
}

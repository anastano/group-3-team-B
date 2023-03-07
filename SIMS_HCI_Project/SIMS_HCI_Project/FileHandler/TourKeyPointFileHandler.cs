using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class TourKeyPointFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourKeyPoints.csv";

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

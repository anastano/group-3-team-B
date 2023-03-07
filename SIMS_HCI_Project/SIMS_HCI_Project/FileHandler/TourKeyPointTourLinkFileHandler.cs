using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class TourKeyPointTourLinkFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourKeyPoints_Tours.csv";

        private readonly Serializer<TourKeyPointTourLink> _serializer;

        public TourKeyPointTourLinkFileHandler()
        {
            _serializer = new Serializer<TourKeyPointTourLink>();
        }

        public List<TourKeyPointTourLink> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<TourKeyPointTourLink> tourKeyPointTourLink)
        {
            _serializer.ToCSV(FilePath, tourKeyPointTourLink);
        }
    }
}

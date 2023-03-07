using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourKeyPointTourLinkController
    {
        private TourKeyPointTourLinkFileHandler _fileHandler;

        private readonly List<TourKeyPointTourLink> _tourKeyPointTourLink;

        public TourKeyPointTourLinkController()
        {
            _fileHandler = new TourKeyPointTourLinkFileHandler();
            _tourKeyPointTourLink = _fileHandler.Load();
        }

        public List<TourKeyPointTourLink> GetAll()
        {
            return _tourKeyPointTourLink;
        }

        public TourKeyPointTourLink Save(TourKeyPointTourLink tourKeyPointTourLink)
        {
            _tourKeyPointTourLink.Add(tourKeyPointTourLink);
            _fileHandler.Save(_tourKeyPointTourLink);
            return tourKeyPointTourLink;
        }
    }
}

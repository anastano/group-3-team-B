using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourTimeController
    {
        private TourTimeFileHandler _fileHandler;

        private readonly List<TourTime> _tourTimes;

        public TourTimeController()
        {
            _fileHandler = new TourTimeFileHandler();
            _tourTimes = _fileHandler.Load();
        }

        public List<TourTime> GetAll()
        {
            return _tourTimes;
        }

        public TourTime Save(TourTime tourTime)
        {
            _tourTimes.Add(tourTime);
            _fileHandler.Save(_tourTimes);
            return tourTime;
        }
    }
}

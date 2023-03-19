using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SIMS_HCI_Project.Controller
{
    public class TourKeyPointController
    {
        private TourKeyPointFileHandler _fileHandler;

        private static List<TourKeyPoint> _tourKeyPoints;

        public TourKeyPointController()
        {
            _fileHandler = new TourKeyPointFileHandler();

            if(_tourKeyPoints == null)
            {
                Load();
            }
        }

        public TourKeyPoint GetById(int id) //DELETE WHEN MERGE
        {
            return _tourKeyPoints.Find(tkp => tkp.Id == id);
        }
        public List<TourKeyPoint> GetByIds(List<int> ids) /* DELETE WHEN MERGE */
        {
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();
            foreach (int id in ids)
            {
                tourKeyPoints.Add(GetById(id));
            }

            return tourKeyPoints;
        }
        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPoints;
        }

        public void Load()
        {
            _tourKeyPoints = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tourKeyPoints);
        }

        public void Add(TourKeyPoint tourKeyPoint)
        {
            tourKeyPoint.Id = GenerateId();
            _tourKeyPoints.Add(tourKeyPoint);
            Save();
        }

        public void AddMultiple(List<TourKeyPoint> tourKeyPoints)
        {
            foreach(TourKeyPoint tourKeyPoint in tourKeyPoints)
            {
                tourKeyPoint.Id = GenerateId();
                _tourKeyPoints.Add(tourKeyPoint);
            }
            Save();
        }
        
        private int GenerateId()
        {
            if (_tourKeyPoints.Count == 0) return 1;
            return _tourKeyPoints[_tourKeyPoints.Count - 1].Id + 1;
        }

        public TourKeyPoint FindById(int id)
        {
            return _tourKeyPoints.Find(tkp => tkp.Id == id);
        }

        public List<TourKeyPoint> FindByIds(List<int> ids) /* TODO: Try to simplify with LINQ */
        {
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();
            foreach(int id in ids)
            {
                tourKeyPoints.Add(FindById(id));
            }

            return tourKeyPoints;
        }
    }
}

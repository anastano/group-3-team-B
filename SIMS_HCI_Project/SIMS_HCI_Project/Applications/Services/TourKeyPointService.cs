using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourKeyPointService
    {
        private readonly ITourKeyPointRepository _tourKeyPointRepository;

        public TourKeyPointService()
        {
            _tourKeyPointRepository = Injector.Injector.CreateInstance<ITourKeyPointRepository>();
        }

        public void Load()
        {
            _tourKeyPointRepository.Load();
        }

        public void Save()
        {
            _tourKeyPointRepository.Save();
        }

        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPointRepository.GetAll();
        }

        public TourKeyPoint FindById(int id)
        {
            return _tourKeyPointRepository.FindById(id);
        }

        public List<TourKeyPoint> FindByIds(List<int> ids)
        {
            return _tourKeyPointRepository.FindByIds(ids);
        }
    }
}

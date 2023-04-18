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
        // delete entire service #New
        private readonly ITourKeyPointRepository _tourKeyPointRepository;

        public TourKeyPointService()
        {
            _tourKeyPointRepository = Injector.Injector.CreateInstance<ITourKeyPointRepository>();
        }

        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPointRepository.GetAll();
        }

        public TourKeyPoint FindById(int id)
        {
            return _tourKeyPointRepository.GetById(id);
        }

        public List<TourKeyPoint> FindByIds(List<int> ids)
        {
            return _tourKeyPointRepository.GetByIds(ids);
        }
    }
}

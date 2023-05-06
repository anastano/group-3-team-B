using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RenovationService
    {
        private readonly IRenovationRepository _renovationRepository;

        public RenovationService()
        {
            _renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
        }

        public Renovation GetById(int id)
        {
            return _renovationRepository.GetById(id);
        }

        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public List<Renovation> GetByOwnerId(int ownerId)
        { 
            return _renovationRepository.GetByOwnerId(ownerId);
        }

        public void ConnectRenovationsWithAccommodations(AccommodationService accommodationService)
        {
            foreach (Renovation renovation in GetAll())
            {
                renovation.Accommodation = accommodationService.GetById(renovation.AccommodationId);
            }
        }

        public void NotifyObservers()
        {
            _renovationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _renovationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _renovationRepository.Unsubscribe(observer);
        }
    }
}

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
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;

        public AccommodationReservationService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public AccommodationReservation FindById(int id)
        {
            return _reservationRepository.FindById(id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public void Load()
        {
            _reservationRepository.Load();
        }

        public void Save()
        {
            _reservationRepository.Save();
        }


        public void NotifyObservers()
        {
            _reservationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _reservationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _reservationRepository.Unsubscribe(observer);
        }


    }
}

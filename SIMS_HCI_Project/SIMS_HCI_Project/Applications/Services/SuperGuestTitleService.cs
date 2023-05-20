using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class SuperGuestTitleService
    {
        private readonly ISuperGuestTitleRepository _titleRepository;
        public SuperGuestTitleService()
        {
            _titleRepository = Injector.Injector.CreateInstance<ISuperGuestTitleRepository>();
        }
        public SuperGuestTitle GetGuestActiveTitle(int gudestId)
        {
            return _titleRepository.GetGuestActiveTitle(gudestId);
        }
        public List<SuperGuestTitle> GetAll()
        {
            return _titleRepository.GetAll();
        }
        public void UpdateTitles(AccommodationReservationService accommodationReservationService)
        {
            foreach (SuperGuestTitle title in _titleRepository.GetExpiredActiveTitles())
            {
                if (IsSuperGuestConditionFulfilled(accommodationReservationService, title.Guest, title.ActivationDate))
                {
                    _titleRepository.Add(new SuperGuestTitle(title.Guest, title.ActivationDate.AddYears(1)));
                }
            }
        }
        public void ConvertActiveTitlesIntoExpired(DateTime currentDate)
        {
            _titleRepository.ConvertActiveTitlesIntoExpired(currentDate);
        }
        public bool IsSuperGuest(Guest1 guest)
        {
            return _titleRepository.GetGuestActiveTitle(guest.Id) == null ? false : true;
        }
        public void UpdateSuperGuestTitle(AccommodationReservationService accommodationReservationService, Guest1 guest)
        {
            if (IsSuperGuest(guest))
            {
                _titleRepository.UpdateAvailablePoints(guest.Id);
            }
            else
            {
                if (IsSuperGuestConditionFulfilled(accommodationReservationService, guest, DateTime.Today.AddYears(-1)))
                {
                    _titleRepository.Add(new SuperGuestTitle(guest));
                }
            }
        }
        private bool IsSuperGuestConditionFulfilled(AccommodationReservationService accommodationReservationService, Guest1 guest, DateTime start)
        {
            return accommodationReservationService.GetReservationsWithinOneYear(guest.Id, start).Count >= 10 ? true : false;
        }
    }
}

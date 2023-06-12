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
        private readonly IAccommodationReservationRepository _reservationRepository;
        public SuperGuestTitleService()
        {
            _titleRepository = Injector.Injector.CreateInstance<ISuperGuestTitleRepository>();
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }
        public SuperGuestTitle GetGuestActiveTitle(int gudestId)
        {
            return _titleRepository.GetGuestActiveTitle(gudestId);
        }
        public void UpdateTitles()
        {
            foreach (SuperGuestTitle title in _titleRepository.GetExpiredActiveTitles())
            {
                if (IsSuperGuestConditionFulfilled(title.Guest))
                {
                    _titleRepository.Add(new SuperGuestTitle(title.Guest, title.ActivationDate.AddYears(1)));
                }
            }
        }
        public void ConvertActiveTitlesIntoExpired(DateTime currentDate)
        {
            _titleRepository.ConvertActiveTitlesIntoExpired(currentDate);
        }
        public bool HasSuperGuestTitle(Guest1 guest)
        {
            return _titleRepository.GetGuestActiveTitle(guest.Id) == null ? false : true;
        }
        public void UpdateSuperGuestTitle(Guest1 guest)
        {
            if (HasSuperGuestTitle(guest))
            {
                _titleRepository.UpdateAvailablePoints(guest.Id);
            }
            else if(IsSuperGuestConditionFulfilled(guest))
            {
                _titleRepository.Add(new SuperGuestTitle(guest));
            }
        }
        private bool IsSuperGuestConditionFulfilled(Guest1 guest)
        {
            return _reservationRepository.GetReservationsWithinOneYear(guest.Id).Count >= 10 ? true : false;
        }
    }
}

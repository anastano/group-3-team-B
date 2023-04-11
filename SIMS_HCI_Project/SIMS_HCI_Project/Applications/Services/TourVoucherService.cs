using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourVoucherService
    {
        private readonly ITourVoucherRepository _tourVoucherRepository;

        public TourVoucherService()
        {
            _tourVoucherRepository = Injector.Injector.CreateInstance<ITourVoucherRepository>();
        }

        public void Load()
        {
            _tourVoucherRepository.Load();
        }

        public void Save()
        {
            _tourVoucherRepository.Save();
        }

        public void GiveVouchersToGuestsWithReservation(List<TourReservation> tourReservations)
        {
            _tourVoucherRepository.GiveVouchersToGuestsWithReservation(tourReservations);
        }
    }
}

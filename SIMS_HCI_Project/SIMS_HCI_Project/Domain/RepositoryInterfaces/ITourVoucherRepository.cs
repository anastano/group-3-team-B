using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourVoucherRepository
    {
        void Load();
        void Save();
        void Add(TourVoucher tourVoucher);
        void AddMultiple(List<TourVoucher> tourVouchers);
        void GiveVouchersToGuestsWithReservation(List<TourReservation> tourReservations);
    }
}
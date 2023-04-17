using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;
using SIMS_HCI_Project.Observer;
using System;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourVoucherRepository
    {
        void Add(TourVoucher tourVoucher);
        void AddMultiple(List<TourVoucher> tourVouchers);
        void GiveVouchersToGuestsWithReservation(List<TourReservation> tourReservations);
        TourVoucher GetById(int id);
        List<TourVoucher> GetValidVouchersByGuestId(int id);
        void UseVoucher(TourVoucher selectedVoucher);

        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
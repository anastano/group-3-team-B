using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;
using SIMS_HCI_Project.Observer;
using System;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourVoucherRepository
    {
        TourVoucher GetById(int id);
        void Save();
        void Add(TourVoucher tourVoucher);
        void AddMultiple(List<TourVoucher> tourVouchers); // change to Bulk, not Multiple #New

        List<TourVoucher> GetValidVouchersByGuestId(int id);
        //void UseVoucher(TourVoucher selectedVoucher); // move logic to Service, add Update method for this #New, moved to service

        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;
using SIMS_HCI_Project.Observer;
using System;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourVoucherRepository
    {
        TourVoucher GetById(int id);
        List<TourVoucher> GetAll();
        List<TourVoucher> GetValidVouchersByGuestId(int id);

        void Add(TourVoucher tourVoucher);
        void AddBulk(List<TourVoucher> tourVouchers);
        void Update(TourVoucher tourVoucher);
        void BulkUpdate(List<TourVoucher> tourVouchers);
    }
}
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourVoucherRepository : ITourVoucherRepository
    {
        private TourVoucherFileHandler _fileHandler;
        private static List<TourVoucher> _tourVouchers;

        public TourVoucherRepository()
        {
            _fileHandler = new TourVoucherFileHandler();

            if (_tourVouchers == null)
            {
                Load();
            }
        }

        private void Load()
        {
            _tourVouchers = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_tourVouchers);
        }

        private int GenerateId()
        {
            return _tourVouchers.Count == 0 ? 1 : _tourVouchers[_tourVouchers.Count - 1].Id + 1;
        }

        public TourVoucher GetById(int id)
        {
            return _tourVouchers.Find(v => v.Id == id);
        }

        public List<TourVoucher> GetAll()
        {
            return _tourVouchers;
        }

        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            return _tourVouchers.FindAll(v => v.GuestId == id && v.Status == VoucherStatus.VALID);
        }

        public void Add(TourVoucher tourVoucher)
        {
            tourVoucher.Id = GenerateId();
            _tourVouchers.Add(tourVoucher);

            Save();
        }

        public void AddBulk(List<TourVoucher> tourVouchers)
        {
            foreach (TourVoucher tourVoucher in tourVouchers)
            {
                tourVoucher.Id = GenerateId();
                _tourVouchers.Add(tourVoucher);
            }
            Save();
        }

        public void Update(TourVoucher tourVoucher)
        {
            TourVoucher tourVoucherUpdated = GetById(tourVoucher.Id);
            tourVoucherUpdated = tourVoucher;

            Save();
        }

        public void BulkUpdate(List<TourVoucher> tourVouchers)
        {
            foreach (TourVoucher tourVoucher in tourVouchers)
            {
                TourVoucher toUpdate = GetById(tourVoucher.Id);
                toUpdate = tourVoucher;
            }
            Save();
        }
    }
}

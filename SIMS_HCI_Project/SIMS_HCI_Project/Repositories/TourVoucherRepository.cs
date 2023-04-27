using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourVoucherRepository : ITourVoucherRepository, ISubject
    {
        private TourVoucherFileHandler _fileHandler;
        private static List<TourVoucher> _tourVouchers;
        private readonly int DefaultExpirationDays = 10;
        private readonly List<IObserver> _observers;


        public TourVoucherRepository()
        {
            _fileHandler = new TourVoucherFileHandler();

            if (_tourVouchers == null)
            {
                Load();
            }
            _observers = new List<IObserver>();
        }

        public void Load()
        {
            _tourVouchers = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tourVouchers);
        }

        public void Add(TourVoucher tourVoucher)
        {
            tourVoucher.Id = GenerateId();
            _tourVouchers.Add(tourVoucher);

            Save();
        }

        public void AddMultiple(List<TourVoucher> tourVouchers)
        {
            foreach (TourVoucher tourVoucher in tourVouchers)
            {
                tourVoucher.Id = GenerateId();
                _tourVouchers.Add(tourVoucher);
            }
            Save();
        }

        private int GenerateId()
        {
            return _tourVouchers.Count == 0 ? 1 : _tourVouchers[_tourVouchers.Count - 1].Id + 1;
        }

        public TourVoucher GetById(int id)
        {
            UpdateStatusForExpired();
            return _tourVouchers.Find(v => v.Id == id);
        }

        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            UpdateStatusForExpired();
            return _tourVouchers.FindAll(v => v.GuestId == id && v.Status == VoucherStatus.VALID);
        }

        public void UpdateStatusForExpired()
        {
            foreach(TourVoucher tourVoucher in _tourVouchers)
            {
                if (tourVoucher.ExpirationDate > DateTime.Now)
                    tourVoucher.Status = VoucherStatus.EXPIRED;
            }
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}

using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourVoucherController : ISubject
    {
        private readonly List<IObserver> _observers;
        private TourVoucherFileHandler _fileHandler;

        private static List<TourVoucher> _tourVouchers;
        private readonly int DefaultExpirationDays = 10;

        public TourVoucherController()
        {
            _fileHandler = new TourVoucherFileHandler();

            if (_tourVouchers == null)
            {
                Load();
            }
            _observers = new List<IObserver>();
        }

        public List<TourVoucher> GetAll()
        {
            return _tourVouchers;
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
        public TourVoucher FindById(int id)
        {
            return _tourVouchers.Find(v => v.Id == id);
        }
        public List<TourVoucher> GetAllByGuestId(int id)
        {
            return _tourVouchers.FindAll(v => v.GuestId == id);
        }
        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            return _tourVouchers.FindAll(v => v.GuestId == id && v.Status==VoucherStatus.VALID);
        }
        private int GenerateId()
        {
            if (_tourVouchers.Count == 0) return 1;
            return _tourVouchers[_tourVouchers.Count - 1].Id + 1;
        }

        public void GiveVouchersToGuestsWithReservation(List<TourReservation> tourReservations)
        {
            List<TourVoucher> givenTourVouchers = new List<TourVoucher>();

            foreach (TourReservation tourReservation in tourReservations)
            {
                tourReservation.Status = TourReservationStatus.CANCELLED;
                givenTourVouchers.Add(new TourVoucher(tourReservation.Guest2Id,"generate_name_later", DateTime.Now, DateTime.Now.AddDays(DefaultExpirationDays)));
            }

            AddMultiple(givenTourVouchers);
        }

        public void UseVoucher(TourVoucher selectedVoucher)
        {
            if (selectedVoucher == null) return;

            TourVoucher voucher = FindById(selectedVoucher.Id);
            voucher.Status = VoucherStatus.USED;
            Save();
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}

using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class TourVoucherController
    {
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
                givenTourVouchers.Add(new TourVoucher(tourReservation.Guest2Id, DateTime.Now, DateTime.Now.AddDays(DefaultExpirationDays)));
            }

            AddMultiple(givenTourVouchers);
        }
    }
}

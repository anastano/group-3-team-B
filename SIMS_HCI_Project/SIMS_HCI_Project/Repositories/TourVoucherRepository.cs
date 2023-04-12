using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourVoucherRepository : ITourVoucherRepository
    {
        private TourVoucherFileHandler _fileHandler;
        private static List<TourVoucher> _tourVouchers;
        private readonly int DefaultExpirationDays = 10;

        public TourVoucherRepository()
        {
            _fileHandler = new TourVoucherFileHandler();

            if (_tourVouchers == null)
            {
                Load();
            }
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
                givenTourVouchers.Add(new TourVoucher(tourReservation.Guest2Id, "generate_name_later", DateTime.Now, DateTime.Now.AddDays(DefaultExpirationDays)));
            }

            AddMultiple(givenTourVouchers);
        }
    }
}

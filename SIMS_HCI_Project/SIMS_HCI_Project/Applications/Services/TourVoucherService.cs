using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Observer;
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

        public TourVoucher GetById(int id)
        {
            return _tourVoucherRepository.GetById(id);
        }

        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            return _tourVoucherRepository.GetValidVouchersByGuestId(id);
        }

        public void UseVoucher(TourVoucher selectedVoucher)
        {
            if(selectedVoucher == null) return;

            TourVoucher voucher = GetById(selectedVoucher.Id);
            voucher.Use();
            _tourVoucherRepository.Save();
        }

        public void NotifyObservers()
        {
            _tourVoucherRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _tourVoucherRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourVoucherRepository.Unsubscribe(observer);
        }
    }
}

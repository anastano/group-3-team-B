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
            UpdateStatusForExpired();
            return _tourVoucherRepository.GetById(id);
        }

        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            UpdateStatusForExpired();
            return _tourVoucherRepository.GetValidVouchersByGuestId(id);
        }

        public void UseVoucher(TourVoucher selectedVoucher)
        {
            if(selectedVoucher == null) return;

            TourVoucher voucher = GetById(selectedVoucher.Id);
            voucher.Use();
            _tourVoucherRepository.Update(voucher);
        }

        private void UpdateStatusForExpired()
        {
            foreach (TourVoucher tourVoucher in _tourVoucherRepository.GetAll())
            {
                if (tourVoucher.ExpirationDate > DateTime.Now)
                {
                    tourVoucher.End();
                    _tourVoucherRepository.Update(tourVoucher);
                }
            }
        }
    }
}

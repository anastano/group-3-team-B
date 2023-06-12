using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

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
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public TourVoucherService()
        {
            _tourVoucherRepository = Injector.Injector.CreateInstance<ITourVoucherRepository>();
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }

        public TourVoucher GetById(int id)
        {
            UpdateStatusForExpired();
            return _tourVoucherRepository.GetById(id);
        }

        public List<TourVoucher> GetValidVouchersByGuestId(int id)
        {
            List<TourVoucher> vouchersBefore = _tourVoucherRepository.GetValidVouchersByGuestId(id);
            UpdateStatusForExpired();
            List<TourVoucher> vouchersAfter = _tourVoucherRepository.GetValidVouchersByGuestId(id);

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
                if (tourVoucher.ExpirationDate < DateTime.Now)
                {
                    tourVoucher.End();
                    _tourVoucherRepository.Update(tourVoucher);
                }
            }
        }

        public void WinVoucherForLoyalty(int guestId)
        {
            int attendancesCount = _guestTourAttendanceRepository.GetGuestAttendancesCountLastYear(guestId);
            if(attendancesCount > 5 && !_tourVoucherRepository.HasLoyaltyVoucher(guestId))
            {
                _tourVoucherRepository.Add(new TourVoucher(guestId, "LOYALTY VOUCHER", DateTime.Now, DateTime.Now.AddMonths(6)));
                NotificationService notificationService = new NotificationService();
                string Message = "NEW VOUCHER - You won voucher for loyalty, which can be used for any tour. It expires in 6 months.";
                notificationService.Add(new Notification(Message, guestId, false, NotificationType.DEFAULT));
            }
        } 
    }
}

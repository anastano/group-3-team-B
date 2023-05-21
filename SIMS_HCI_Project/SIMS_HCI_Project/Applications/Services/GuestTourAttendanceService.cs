using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIMS_HCI_Project.Applications.Services
{
    public class GuestTourAttendanceService
    {
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ITourReservationRepository _tourReservationRepository;

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, tourTimeId);
        }
      
        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId)  // Simplify with LINQ
        {
            List<TourTime> tourTimes = new List<TourTime>();
            foreach (var gta in _guestTourAttendanceRepository.GetAllByGuestId(guestId))
            {
                if (gta.Status == AttendanceStatus.PRESENT)
                {
                    tourTimes.Add(_tourTimeRepository.GetById(gta.TourTimeId));
                }
            }
            return tourTimes;
        }

        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId) //old
        {
            GuestTourAttendance attendance = _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, tourTimeId);
            if (attendance.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
            {
                attendance.Status = AttendanceStatus.PRESENT;
                _guestTourAttendanceRepository.Update(attendance);
            }
        }


        public List<GuestTourAttendance> GetWithConfirmationRequestedStatus(int guestId)
        {
            List<TourReservation> reservations = _tourReservationRepository.GetAllByGuestIdAndTourId(guestId, tourId);
            foreach(TourReservation reservation in reservations)
            {
                GuestTourAttendance attendance = _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, reservation.TourTimeId);
                if (attendance.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
                {
                    attendance.Status = AttendanceStatus.PRESENT;
                    _guestTourAttendanceRepository.Update(attendance);
                }
            }

            //List<TourReservation> rees = _tourReservationRepository.GetAllByGuestId(guestId);
        }

        public List<GuestTourAttendance> GetByConfirmationRequestedStatus(int guestId)
        {
            return _guestTourAttendanceRepository.GetWithConfirmationRequestedStatus(guestId);
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            guestTourAttendance.KeyPointJoined = guestTourAttendance.TourReservation.TourTime.CurrentKeyPoint;
            guestTourAttendance.KeyPointJoinedId = guestTourAttendance.TourReservation.TourTime.CurrentKeyPoint.Id;
            _guestTourAttendanceRepository.Update(guestTourAttendance);
            NotificationService notificationService = new NotificationService();
            string Message = "You have request to confirm your attendance for tour with id: [" + guestTourAttendance.TourTimeId + "].";
            notificationService.Add(new Notification(Message, guestTourAttendance.GuestId, false, NotificationType.CONFIRM_ATTENDANCE));
        }
    }

}

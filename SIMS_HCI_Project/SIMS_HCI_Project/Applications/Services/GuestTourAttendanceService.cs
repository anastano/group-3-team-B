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

        public GuestTourAttendance GetById(int id)
        {
            return _guestTourAttendanceRepository.GetById(id);
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendanceRepository.GetAll();
        }
        
        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendanceRepository.GetAllByTourId(id);
        }

        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, tourTimeId);
        }
        public List<GuestTourAttendance> GetByGuestAndLocationIds(int guestId, int locationId)
        {
            return _guestTourAttendanceRepository.GetAllByGuestAndLocationIds(guestId, locationId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId)  // Simplify with LINQ
        {
            List<TourTime> tourTimes = new List<TourTime>();
            foreach (var gta in _guestTourAttendanceRepository.GetAllByGuestId(guestId))
            {
                if (gta.Status == AttendanceStatus.PRESENT)
                {
                    tourTimes.Add(_tourTimeRepository.GetById(gta.TourReservation.TourTimeId));
                }
            }
            return tourTimes;
        }

        public List<GuestTourAttendance> GetWithConfirmationRequestedStatus(int guestId)
        {
            return _guestTourAttendanceRepository.GetWithConfirmationRequestedStatus(guestId);
        }

        public void Add(GuestTourAttendance guestTourAttendance)
        {
            _guestTourAttendanceRepository.Add(guestTourAttendance);
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId) // AN: why not just attendanceId?
        {
            List<TourReservation> reservations = _tourReservationRepository.GetAllByGuestIdAndTourId(guestId, tourTimeId);
            foreach (TourReservation reservation in reservations)
            {
                GuestTourAttendance attendance = _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, reservation.TourTimeId);
                if (attendance.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
                {
                    attendance.Status = AttendanceStatus.PRESENT;
                    _guestTourAttendanceRepository.Update(attendance);
                }
            }
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.RequestConfirmation();
            _guestTourAttendanceRepository.Update(guestTourAttendance);
            NotificationService notificationService = new NotificationService();
            string Message = "You have request to confirm your attendance for tour with id: [" + guestTourAttendance.TourReservation.TourTimeId + "].";
            notificationService.Add(new Notification(Message, guestTourAttendance.TourReservation.GuestId, false, NotificationType.CONFIRM_ATTENDANCE));
        }
    }

}

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

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
        }

        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, tourTimeId);
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

        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId)
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
            return _guestTourAttendanceRepository.GetWithConfirmationRequestedStatus(guestId);
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            guestTourAttendance.KeyPointJoined = guestTourAttendance.TourReservation.TourTime.CurrentKeyPoint;
            guestTourAttendance.KeyPointJoinedId = guestTourAttendance.TourReservation.TourTime.CurrentKeyPoint.Id;
            _guestTourAttendanceRepository.Update(guestTourAttendance);
        }
    }

}

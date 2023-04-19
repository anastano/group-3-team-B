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

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourService tourService)
        {
            return _guestTourAttendanceRepository.GetTourTimesWhereGuestWasPresent(guestId, tourService);
        }

        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId)
        {
            _guestTourAttendanceRepository.ConfirmAttendanceForTourTime(guestId, tourTimeId);
        }

        public List<GuestTourAttendance> GetByConfirmationRequestedStatus(int guestId)
        {
            return _guestTourAttendanceRepository.GetByConfirmationRequestedStatus(guestId);
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            _guestTourAttendanceRepository.Update(guestTourAttendance);
        }
    }

}

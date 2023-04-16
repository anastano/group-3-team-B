using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class GuestTourAttendanceService
    {
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }

        public void Load()
        {
            _guestTourAttendanceRepository.Load();
        }

        public void Save()
        {
            _guestTourAttendanceRepository.Save();
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendanceRepository.GetAll();
        }
        public GuestTourAttendance FindById(int id)
        {
            return _guestTourAttendanceRepository.FindById(id);
        }

        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendanceRepository.GetAllByTourId(id);
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourTimeService tourTimeService)
        {
            return _guestTourAttendanceRepository.GetTourTimesWhereGuestWasPresent(guestId, tourTimeService);
        }
    }

}

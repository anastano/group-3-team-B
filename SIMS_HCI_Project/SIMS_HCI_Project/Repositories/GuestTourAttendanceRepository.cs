using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Applications.Services;

namespace SIMS_HCI_Project.Repositories
{
    public class GuestTourAttendanceRepository : IGuestTourAttendanceRepository
    {
        private GuestTourAttendanceFileHandler _fileHandler;
        private static List<GuestTourAttendance> _guestTourAttendances;

        //private TourTimeService tourtimeservice = new TourTimeService(); // skloni, treba samo za testiranje

        public GuestTourAttendanceRepository()
        {
            _fileHandler = new GuestTourAttendanceFileHandler();
            if (_guestTourAttendances == null)
            {
                Load();
            }
        }
        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendances;
        }

        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourTimeId == id);
        }

        public GuestTourAttendance FindById(int id)
        {
            return _guestTourAttendances.Find(gta => gta.Id == id);
        }

        public void Load()
        {
            _guestTourAttendances = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_guestTourAttendances);
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            GuestTourAttendance attendance = GetByGuestAndTourTimeIds(guestId, tourTimeId);
            return _guestTourAttendances.Any(gta => gta.Status == AttendanceStatus.PRESENT);
        }
        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendances.Find(g => g.GuestId == guestId && g.TourTimeId == tourTimeId);
        }

        public List<GuestTourAttendance> GetAllByGuestId( int guestId)
        {
            return _guestTourAttendances.FindAll(g => g.GuestId == guestId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourTimeService tourTimeService) //prosledi ttservice
        {
            List<TourTime> tourTimes = new List<TourTime>();
            foreach(var gta in GetAllByGuestId(guestId))
            {
                if(gta.Status == AttendanceStatus.PRESENT)
                {
                    tourTimes.Add(tourTimeService.GetById(gta.TourTimeId));
                }
            }
            return tourTimes;
        }

        
    }
}

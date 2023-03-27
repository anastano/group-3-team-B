using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class GuestTourAttendanceController
    {
        private GuestTourAttendanceFileHandler _fileHandler;

        private TourReservationController _tourReservationController;
        private static List<GuestTourAttendance> _guestTourAttendances;

        public GuestTourAttendanceController()
        {
            _fileHandler = new GuestTourAttendanceFileHandler();
            _tourReservationController = new TourReservationController();

            if (_guestTourAttendances == null)
            {
                Load();
            }
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendances;
        }

        public void Load()
        {
            _guestTourAttendances = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_guestTourAttendances);
        }

        public void Add(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Id = GenerateId();
            _guestTourAttendances.Add(guestTourAttendance);
            Save();
        }

        public int GenerateId()
        {
            if (_guestTourAttendances.Count == 0) return 1;
            return _guestTourAttendances[_guestTourAttendances.Count - 1].Id + 1;
        }

        public GuestTourAttendance FindById(int id)
        {
            return _guestTourAttendances.Find(gta => gta.Id == id);
        }

        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourTimeId == id);
        }

        public void GenerateAttendancesByTour(TourTime tourTime)
        {
            List<int> guestIds = GetGuestsWithReservation(tourTime);

            foreach (int guestId in guestIds)
            {
                GuestTourAttendance newGuestTourAttendance = new GuestTourAttendance(guestId, tourTime.Id);
                newGuestTourAttendance.TourTime = tourTime;
                Add(newGuestTourAttendance);
            }
        }

        public List<int> GetGuestsWithReservation(TourTime tourTime)
        {
            return _tourReservationController.GetAllByTourTimeId(tourTime.Id).Select(c => c.Guest2Id).ToList();
        }

        public void UpdateGuestStatusesAfterTourEnd(TourTime tourTime)
        {
            foreach (GuestTourAttendance guestTourAttendance in tourTime.GuestAttendances)
            {
                if (guestTourAttendance.Status != AttendanceStatus.PRESENT)
                {
                    guestTourAttendance.Status = AttendanceStatus.NEVER_SHOWED_UP;
                }
            }
            Save();
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            Save();
        }
    }
}

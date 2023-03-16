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

        private List<GuestTourAttendance> _guestTourAttendances;

        public GuestTourAttendanceController()
        {
            _fileHandler = new GuestTourAttendanceFileHandler();
            _guestTourAttendances = _fileHandler.Load();
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendances;
        }

        public void Save(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Id = GenerateId();
            _guestTourAttendances.Add(guestTourAttendance);
            _fileHandler.Save(_guestTourAttendances);
        }

        public List<GuestTourAttendance> GetByTourId(int id)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourTimeId == id);
        }

        public GuestTourAttendance FindById(int id)
        {
            return _guestTourAttendances.Find(gta => gta.Id == id);
        }

        public int GenerateId()
        {
            if (_guestTourAttendances.Count == 0) return 1;
            return _guestTourAttendances[_guestTourAttendances.Count - 1].Id + 1;
        }

        public void GenerateByTour(TourTime tourTime) /* TODO: After Tour Reservation is done, refactor this and maybe move to Reservation Controller */
        {
            //Temporary, here we should load all Guests that reserved the Tour 
            //And we will have class guest to iterate through it

            UserController userController = new UserController();
            List<User> allGuests = userController.GetAll().FindAll(u => u.Id[0] == 'S');
            foreach (User user in allGuests)
            {
                GuestTourAttendance newGuestTourAttendance = new GuestTourAttendance(user.Id, tourTime.Id);
                newGuestTourAttendance.TourTime = tourTime;
                Save(newGuestTourAttendance);
            }
        }

        public void UpdateAfterTourEnd(TourTime tourTime)
        {
            foreach (GuestTourAttendance guestTourAttendance in tourTime.GuestAttendances)
            {
                if (guestTourAttendance.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
                {
                    guestTourAttendance.Status = AttendanceStatus.NEVER_SHOWED_UP;
                }
            }
            _fileHandler.Save(_guestTourAttendances);
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            FindById(guestTourAttendance.Id).Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            _fileHandler.Save(_guestTourAttendances);
        }
    }
}

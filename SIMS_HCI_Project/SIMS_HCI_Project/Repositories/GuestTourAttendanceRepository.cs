using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Domain.DTOs;


namespace SIMS_HCI_Project.Repositories
{
    public class GuestTourAttendanceRepository : IGuestTourAttendanceRepository
    {
        private GuestTourAttendanceFileHandler _fileHandler;
        private static List<GuestTourAttendance> _guestTourAttendances;

        public GuestTourAttendanceRepository()
        {
            _fileHandler = new GuestTourAttendanceFileHandler();

            if (_guestTourAttendances == null)
            {
                Load();
            }
        }

        private void Load()
        {
            _guestTourAttendances = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_guestTourAttendances);
        }

        private int GenerateId()
        {
            return _guestTourAttendances.Count == 0 ? 1 : _guestTourAttendances[_guestTourAttendances.Count - 1].Id + 1;
        }

        public GuestTourAttendance GetById(int id)
        {
            return _guestTourAttendances.Find(gta => gta.Id == id);
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendances;
        }

        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourReservation.TourTimeId == id);
        }
        
        public List<GuestTourAttendance> GetAllByGuestId(int guestId)
        {
            return _guestTourAttendances.FindAll(g => g.TourReservation.GuestId == guestId);
        }

        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendances.Find(g => g.TourReservation.GuestId == guestId && g.TourReservation.TourTimeId == tourTimeId);
        }
        public List<GuestTourAttendance> GetByGuestAndLocationIds(int guestId, int locationId)
        {
            return _guestTourAttendances.FindAll(g => g.TourReservation.GuestId == guestId && g.Status == AttendanceStatus.PRESENT && g.TourReservation.TourTime.Tour.LocationId == locationId );
        }

        public List<GuestTourAttendance> GetWithConfirmationRequestedStatus(int guestId)
        {
            var result = new List<GuestTourAttendance>();
            foreach(var gta in GetAllByGuestId(guestId))
            {
                if(gta.Status == AttendanceStatus.CONFIRMATION_REQUESTED && gta.TourReservation.TourTime.IsInProgress)
                {
                    result.Add(gta);
                }
            }
            return result;
        }
        
        public void Add(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Id = GenerateId();
            _guestTourAttendances.Add(guestTourAttendance);

            Save();
        }

        public void AddBulk(List<GuestTourAttendance> guestTourAttendances)
        {
            foreach (GuestTourAttendance guestTourAttendance in guestTourAttendances)
            {
                guestTourAttendance.Id = GenerateId();
                _guestTourAttendances.Add(guestTourAttendance);
            }
            Save();
        }

        public void Update(GuestTourAttendance guestTourAttendance)
        {
            GuestTourAttendance toUpdate = GetById(guestTourAttendance.Id);
            toUpdate = guestTourAttendance;
            Save();
        }

        public void BulkUpdate(List<GuestTourAttendance> guestTourAttendances)
        {
            foreach (GuestTourAttendance guestTourAttendance in guestTourAttendances)
            {
                GuestTourAttendance toUpdate = GetById(guestTourAttendance.Id);
                toUpdate = guestTourAttendance;
            }
            Save();
        }

        public int GetGuestCountByAgeGroup(AgeGroup ageGroup, int tourTimeId)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourReservation.Guest.Age >= ageGroup.MinAge && gta.TourReservation.Guest.Age <= ageGroup.MaxAge && gta.TourReservation.TourTimeId == tourTimeId).Count;
        }

        public int GetGuestsWithVoucherCount(int tourTimeId)
        {
            return _guestTourAttendances.Where(gta => gta.TourReservation.VoucherUsedId != -1 && gta.TourReservation.TourTimeId == tourTimeId).Count();
        }

        public bool IsPresent(int guestId, int tourTimeId) // move to Guest class
        {
            GuestTourAttendance attendance = GetByGuestAndTourTimeIds(guestId, tourTimeId);
            return _guestTourAttendances.Any(gta => gta.Status == AttendanceStatus.PRESENT);
        }
    }
}

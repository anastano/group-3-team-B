using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _guestTourAttendances.FindAll(gta => gta.TourTimeId == id);
        }

        public int GetGuestNumberByAgeGroup(AgeGroup ageGroup, int tourTimeId)
        {
            return _guestTourAttendances.FindAll(gta => gta.Guest.Age >= ageGroup.MinAge && gta.Guest.Age <= ageGroup.MaxAge && gta.TourTimeId == tourTimeId).Count;
        }

        public int GetTopTourIdByGuestNumber()
        {
            return _guestTourAttendances.Where(gta => gta.TourTime.Status == TourStatus.COMPLETED).GroupBy(gta => gta.TourTimeId).OrderByDescending(t => t.Count()).First().First().TourTimeId;
        }

        public int GetTopTourIdByGuestNumberAndYear(int year)
        {
            return _guestTourAttendances.Where(why => why.TourTime.DepartureTime.Year == year && why.TourTime.Status == TourStatus.COMPLETED).ToList().GroupBy(gta => gta.TourTimeId).OrderByDescending(t => t.Count()).First().First().TourTimeId;
        }
    }
}

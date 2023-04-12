using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
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
    }
}

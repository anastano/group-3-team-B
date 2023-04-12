using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
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

        public void Load()
        {
            _guestTourAttendanceRepository.Load();
        }

        public void Save()
        {
            _guestTourAttendanceRepository.Save();
        }

        public void Add(GuestTourAttendance guestTourAttendance)
        {
            _guestTourAttendanceRepository.Add(guestTourAttendance);
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
    }
}

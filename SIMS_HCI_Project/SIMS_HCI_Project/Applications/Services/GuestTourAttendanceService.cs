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
        private readonly IUserRepository _userRepository;
        private readonly ITourTimeRepository _tourTimeRepository;

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
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

        public void LoadConnections()
        {
            ConnectTours();
            ConnectGuests();
        }

        private void ConnectGuests()
        {
            foreach (GuestTourAttendance guestTourAttendance in _guestTourAttendanceRepository.GetAll())
            {
                guestTourAttendance.Guest = new Guest2(_userRepository.GetById(guestTourAttendance.GuestId));
            }
        }

        private void ConnectTours()
        {
            foreach (GuestTourAttendance guestTourAttendance in _guestTourAttendanceRepository.GetAll())
            {
                guestTourAttendance.TourTime = _tourTimeRepository.GetById(guestTourAttendance.TourTimeId);
            }
        }

        public TourStatisticsInfo GetTourStatistics(int tourTimeId)
        {
            // move this to some global settings or similar?
            List<AgeGroup> wantedAgeGroups = new List<AgeGroup> { new AgeGroup(0, 18), new AgeGroup(18, 50), new AgeGroup(50, 150) };
            Dictionary<AgeGroup, int> guestNumberByAgeGroup = new Dictionary<AgeGroup, int>();

            foreach (AgeGroup ageGroup in wantedAgeGroups)
            {
                guestNumberByAgeGroup.Add(ageGroup, _guestTourAttendanceRepository.GetGuestNumberByAgeGroup(ageGroup, tourTimeId));
            }

            return new TourStatisticsInfo(guestNumberByAgeGroup, 30);
        }

        public TourTime GetTopTour()
        {
            int topTourId = _guestTourAttendanceRepository.GetTopTourIdByGuestNumber();

            return _tourTimeRepository.GetById(topTourId);
        }

        public TourTime GetTopTourByYear(int year)
        {
            int topTourId = _guestTourAttendanceRepository.GetTopTourIdByGuestNumberAndYear(year);

            return _tourTimeRepository.GetById(topTourId);
        }
    }
}

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

        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITourTimeRepository _tourTimeRepository;


        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();

            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
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

        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourService tourService)
        {
            return _guestTourAttendanceRepository.GetTourTimesWhereGuestWasPresent(guestId, tourService);
        }

        public void LoadConnections()
        {
            ConnectTours();
            ConnectGuests();
            ConnectReservations();
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

        private void ConnectReservations()
        {
            foreach (GuestTourAttendance guestTourAttendance in _guestTourAttendanceRepository.GetAll())
            {
                guestTourAttendance.TourReservation = _tourReservationRepository.GetByGuestAndTour(guestTourAttendance.GuestId, guestTourAttendance.TourTimeId);
            }
        }
    }

}

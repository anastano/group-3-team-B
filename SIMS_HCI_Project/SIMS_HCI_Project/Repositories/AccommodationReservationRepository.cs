using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private readonly AccommodationReservationFileHandler _fileHandler;

        private static List<AccommodationReservation> _reservations;

        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler(); 
            if(_reservations == null)
            {
                _reservations = _fileHandler.Load();
            }
        }
        public void ConvertReservedReservationIntoCompleted(DateTime currentDate)
        {
            foreach (var reservation in _reservations)
            {
                if (reservation.End < currentDate && reservation.Status == AccommodationReservationStatus.RESERVED)
                {
                    reservation.Status = AccommodationReservationStatus.COMPLETED;
                }
            }
            Save();
        }
        public int GenerateId()
        {
            return _reservations.Count == 0 ? 1 : _reservations[_reservations.Count - 1].Id + 1;
        }
        public void Save()
        {
            _fileHandler.Save(_reservations);
        }

        public AccommodationReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }

        public List<AccommodationReservation> GetByOwnerId(int ownerId)
        {
            return _reservations.FindAll(r => r.Accommodation.OwnerId == ownerId);
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _reservations.FindAll(r => r.Accommodation.Id == accommodationId);
        }

        public List<AccommodationReservation> GetByGuestId(int guestId)
        {
            return _reservations.FindAll(r => r.GuestId == guestId);
        }

        public List<AccommodationReservation> GetAllReservedByAccommodationId(int accommodationId)
        {
            List<AccommodationReservation>  lista = _reservations.FindAll(r => (r.AccommodationId == accommodationId && r.Status == AccommodationReservationStatus.RESERVED));
            return lista;
        }
        public List<AccommodationReservation> GetAllByStatusAndGuestId(int guestId, AccommodationReservationStatus status)
        {
            return _reservations.FindAll(r => r.GuestId == guestId && r.Status == status).OrderByDescending(r => r.Accommodation.Owner.SuperFlag).ToList();
        }

        public int GetReservationCountByYearAndAccommodationId(int year, int accommodationId) 
        { 
            return GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year).Count();
        }

        public int GetCancellationCountByYearAndAccommodationId(int year, int accommodationId)
        {
           return  GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year && r.Status == AccommodationReservationStatus.CANCELLED).Count();
        }

        public int GetReservationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year && r.Start.Month == monthIndex).Count();
        }

        public int GetCancellationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId) 
        {
            return GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year && r.Start.Month == monthIndex
                                         && r.Status == AccommodationReservationStatus.CANCELLED).Count();
        }

        public void Add(AccommodationReservation reservation)
        {
            reservation.Id = GenerateId();
            _reservations.Add(reservation);
            Save();
        }
        public void EditStatus(int reservationId, AccommodationReservationStatus status)
        {
            AccommodationReservation reservation = _reservations.Find(r => r.Id == reservationId);
            reservation.Status = status;
            Save();
        }

        public void EditReservation(RescheduleRequest request)
        {
            AccommodationReservation reservation = _reservations.Find(r => r.Id == request.AccommodationReservationId);
            reservation.Start = request.WantedStart;
            reservation.End = request.WantedEnd;
            Save();
        }

        public List<AccommodationReservation> OwnerSearch(string accommodationName, string guestName, string guestSurname, int ownerId)
        {
            List<AccommodationReservation> reservations = GetByOwnerId(ownerId);

            var filtered = from _reservation in reservations
                           where (string.IsNullOrEmpty(accommodationName) || _reservation.Accommodation.Name.ToLower().Contains(accommodationName.ToLower()))
                           && (string.IsNullOrEmpty(guestName) || _reservation.Guest.Name.ToLower().Contains(guestName.ToLower()))
                           && (string.IsNullOrEmpty(guestSurname) || _reservation.Guest.Surname.ToLower().Contains(guestSurname.ToLower()))
                           select _reservation;

            return filtered.ToList();
        }
        public List<AccommodationReservation> GetReservationsWithinOneYear(int guestId)
        {
            return _reservations.FindAll(r => r.GuestId == guestId && r.Start >= DateTime.Today.AddYears(-1));
        }

    }
}

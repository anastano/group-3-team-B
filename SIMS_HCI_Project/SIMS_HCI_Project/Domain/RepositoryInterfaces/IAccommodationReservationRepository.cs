using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationReservationRepository
    {
        List<AccommodationReservation> GetAll();
        AccommodationReservation GetById(int id);
        List<AccommodationReservation> GetByOwnerId(int id);
        List<AccommodationReservation> GetByGuestId(int id);
        List<AccommodationReservation> GetAllReservedByAccommodationId(int accommodationId);
        List<AccommodationReservation> GetAllByStatusAndGuestId(int guestId, AccommodationReservationStatus status);
        List<AccommodationReservation> GetReservationsWithinOneYear(int guestId, DateTime start);
        void Add(AccommodationReservation reservation);
        void EditStatus(int id, AccommodationReservationStatus status);
        void EditReservation(RescheduleRequest request);
        void ConvertReservedReservationIntoCompleted(DateTime currentDate);
        List<AccommodationReservation> OwnerSearch(string accommodationName, string guestName, string guestSurname, int ownerId);
        List<AccommodationReservation> GetByAccommodationId(int accommodationId);
        int GetReservationCountByYearAndAccommodationId(int year, int accommodationId);
        int GetCancellationCountByYearAndAccommodationId(int year, int accommodationId);
        int GetReservationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId);
        int GetCancellationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId);
    }
}
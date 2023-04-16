using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;


namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    internal interface IGuestTourAttendanceRepository
    {
        void Load();
        void Save();
        List<GuestTourAttendance> GetAll();
        List<GuestTourAttendance> GetAllByTourId(int id);
        GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId); //check later
        List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourTimeService tourTimeService); 
        GuestTourAttendance GetById(int id);
        int GenerateId();
        void Add(GuestTourAttendance guestTourAttendance);
        int GetGuestCountByAgeGroup(AgeGroup ageGroup, int tourTimeId);
        TourTime GetTourWithMostGuests();
        TourTime GetTourWithMostGuestsByYear(int year);
        int GetGuestsWithVoucherCount(int tourTimeId);
        bool IsPresent(int guestId, int tourTimeId);
    }
}
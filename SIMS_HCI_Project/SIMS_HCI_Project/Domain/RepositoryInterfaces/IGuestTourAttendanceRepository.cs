using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.DTOs;


namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    internal interface IGuestTourAttendanceRepository
    {
        void Load();
        void Save();

        GuestTourAttendance GetById(int id);
        List<GuestTourAttendance> GetAll();
        List<GuestTourAttendance> GetAllByTourId(int id);
        List<GuestTourAttendance> GetAllByGuestId(int id);
        GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId); 
        
        void Add(GuestTourAttendance guestTourAttendance);
        void AddMultiple(List<GuestTourAttendance> guestTourAttendances);
        void Update(GuestTourAttendance guestTourAttendance);
        void BulkUpdate(List<GuestTourAttendance> guestTourAttendances);

        // CLEAN!
        int GetGuestCountByAgeGroup(AgeGroup ageGroup, int tourTimeId);
        int GetGuestsWithVoucherCount(int tourTimeId);
        bool IsPresent(int guestId, int tourTimeId);
        void ConfirmAttendanceForTourTime(int guestId, int tourTimeId);
        List<GuestTourAttendance> GetByConfirmationRequestedStatus(int guestId);
    }
}
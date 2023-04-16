using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Applications.Services;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    internal interface IGuestTourAttendanceRepository
    {
        void Load();
        void Save();
        List<GuestTourAttendance> GetAll();
        List<GuestTourAttendance> GetAllByTourId(int id);
        GuestTourAttendance FindById(int id);
        bool IsPresent(int guestId, int tourTimeId);
        GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId);
        List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourTimeService tourTimeService); //nova, try
    }
}

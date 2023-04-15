using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SIMS_HCI_Project.Domain.Models;

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
    }
}

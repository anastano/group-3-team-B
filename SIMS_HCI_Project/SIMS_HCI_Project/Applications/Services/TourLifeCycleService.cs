using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourLifeCycleService
    {
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public TourLifeCycleService() 
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }
    }
}

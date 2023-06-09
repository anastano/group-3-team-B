using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SIMS_HCI_Project.Applications.Services
{
    internal class CronWannabeService
    {
        private readonly ISuperGuideFlagRepository _superGuideFlagRepository;
        private readonly SuperFlagsService _superFlagsService;

        public CronWannabeService(SuperFlagsService superFlagsService)
        {
            _superFlagsService = superFlagsService;
            _superGuideFlagRepository = Injector.Injector.CreateInstance<ISuperGuideFlagRepository>();

            InitCronJobs();
        }

        private void InitCronJobs()
        {
            Thread thread = new Thread(new ThreadStart(CheckThread));
            thread.Start();
        }

        private void CheckThread()
        {
            System.Timers.Timer timer = new System.Timers.Timer(30 * 1000);
            timer.Elapsed += Callback;
            timer.AutoReset = true;
            timer.Enabled = true;
            Trace.WriteLine("Started CheckThread");
        }

        private void Callback(Object source, ElapsedEventArgs e)
        {
            ReviseSuperGuideFlags();
            Trace.WriteLine("Timer expired");
        }

        private void ReviseSuperGuideFlags()
        {
            foreach (SuperGuideFlag flag in _superGuideFlagRepository.GetAll())
            {
                if(flag.ExpiryDate.Date == DateTime.Today)
                {
                    Trace.WriteLine("REVISED");
                    _superFlagsService.ReviseExpiredSuperGuideFlag(flag.Id);
                }
            }
        }
    }
}

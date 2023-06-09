﻿using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class TourRequestsStatisticsByStatus
    {
        public Dictionary<TourRequestStatus, int> RequestsNumberByStatus { get; set; }

        public TourRequestsStatisticsByStatus(Dictionary<TourRequestStatus, int> requestsNumberByStatus)
        {
            RequestsNumberByStatus = requestsNumberByStatus;
        }
    }
}

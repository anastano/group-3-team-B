﻿using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ISuperGuestTitleRepository
    {
        List<SuperGuestTitle> GetAll();
        List<SuperGuestTitle> GetExpiredActiveTitles();
        SuperGuestTitle GetGuestActiveTitle(int guestId);
        void ConvertActiveTitlesIntoExpired(DateTime currentDate);
        void UpdateAvailablePoints(int guestId);
        void Add(SuperGuestTitle title);

    }
}

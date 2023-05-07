using SIMS_HCI_Project.Domain.Models;
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
        SuperGuestTitle GetByGuestId(int guestId);
        SuperGuestTitle GetGuestActiveTitle(int guestId);
        void ConvertActiveTitlesIntoExpired(DateTime currentDate);

    }
}

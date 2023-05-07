using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class SuperGuestTitleService
    {
        private readonly ISuperGuestTitleRepository _titleRepository;
        public SuperGuestTitleService()
        {
            _titleRepository = Injector.Injector.CreateInstance<ISuperGuestTitleRepository>();
        }
        public SuperGuestTitle GetByGuestId(int id)
        {
            return _titleRepository.GetByGuestId(id);
        }
        public SuperGuestTitle GetGuestActiveTitle(int gudestId)
        {
            return _titleRepository.GetGuestActiveTitle(gudestId);
        }
        public List<SuperGuestTitle> GetAll()
        {
            return _titleRepository.GetAll();
        }
        public void ConnectTitlesWithGuests(Guest1Service guest1Service)
        {
            foreach (SuperGuestTitle title in GetAll())
            {
                title.Guest = guest1Service.GetById(title.GuestId);
            }
        }
        public void ConvertActiveTitlesIntoExpired(DateTime currentDate)
        {
            _titleRepository.ConvertActiveTitlesIntoExpired(currentDate);
        }
    }
}

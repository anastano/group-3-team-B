using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class SuperGuestTitleRepository : ISuperGuestTitleRepository
    {
        private SuperGuestTitleFileHandler _fileHandler;
        private List<SuperGuestTitle> _titles;

        public SuperGuestTitleRepository()
        {
            _fileHandler = new SuperGuestTitleFileHandler();
            _titles = _fileHandler.Load();
        }
        public int GenerateId()
        {
            return _titles.Count == 0 ? 1 : _titles[_titles.Count - 1].Id + 1;
        }
        public void Save()
        {
            _fileHandler.Save(_titles);
        }
        public SuperGuestTitle GetByGuestId(int guestId)
        {
            return _titles.Find(l => l.GuestId == guestId);
        }
        public SuperGuestTitle GetGuestActiveTitle(int guestId)
        {
            return _titles.Find(l => l.GuestId == guestId && l.Status == TitleStatus.ACTIVE);
        }
        public List<SuperGuestTitle> GetAll()
        {
            return _titles;
        }
        public void ConvertActiveTitlesIntoExpired(DateTime currentDate)
        {
            foreach (var title in _titles)
            {
                if (title.ActivationDate.AddYears(1) < currentDate && title.Status == TitleStatus.ACTIVE)
                {
                    title.Status = TitleStatus.EXPIRED;
                }
            }
            Save();
        }
    }
}

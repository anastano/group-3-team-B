using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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
        public SuperGuestTitle GetGuestActiveTitle(int guestId)
        {
            return _titles.Find(l => l.GuestId == guestId && l.Status == TitleStatus.ACTIVE);
        }
        public List<SuperGuestTitle> GetExpiredActiveTitles()
        {
            return _titles.FindAll(l => l.Status == TitleStatus.ACTIVE && l.ActivationDate.AddYears(1) <= DateTime.Now);
        }
        public List<SuperGuestTitle> GetAll()
        {
            return _titles;
        }
        public void Add(SuperGuestTitle title)
        {
            title.Id = GenerateId();
            _titles.Add(title);
            Save();
        }
        public void UpdateAvailablePoints(int guestId)
        {
            SuperGuestTitle guestTitle = GetGuestActiveTitle(guestId);
            guestTitle.AvailablePoints -= 1;
            Save();
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

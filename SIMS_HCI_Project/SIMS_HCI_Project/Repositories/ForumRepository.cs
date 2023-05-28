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
    public class ForumRepository : IForumRepository
    {
        private ForumFileHandler _fileHandler;
        private List<Forum> _forums;

        public ForumRepository()
        {
            _fileHandler = new ForumFileHandler();
            Load();
        }

        private void Load()
        {
            _forums = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_forums);
        }

        private int GenerateId()
        {
            return _forums.Count == 0 ? 1 : _forums[_forums.Count - 1].Id + 1;
        }

        public Forum GetById(int id)
        {
            return _forums.Find(l => l.Id == id);
        }

        public List<Forum> GetAll()
        {
            return _forums;
        }
        public List<Forum> GetByUserId(int userId)
        {
            return _forums.FindAll(f => f.UserId == userId); ;
        }
        public List<Forum> GetForumsExcludingUsers(int userId)
        {
            return _forums.FindAll(f => f.UserId != userId); ;
        }
        public void CloseForum(int id)
        {
            _forums.Find(f => f.Id == id).Status = ForumStatus.CLOSED;
            Save();
        }
    }
}

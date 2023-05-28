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
    public class ForumCommentRepository : IForumCommentRepository
    {
        private ForumCommentFileHandler _fileHandler;
        private List<ForumComment> _comments;

        public ForumCommentRepository()
        {
            _fileHandler = new ForumCommentFileHandler();
            Load();
        }

        private void Load()
        {
            _comments = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_comments);
        }

        private int GenerateId()
        {
            return _comments.Count == 0 ? 1 : _comments[_comments.Count - 1].Id + 1;
        }

        public ForumComment GetById(int id)
        {
            return _comments.Find(l => l.Id == id);
        }

        public List<ForumComment> GetAll()
        {
            return _comments;
        }
    }
}

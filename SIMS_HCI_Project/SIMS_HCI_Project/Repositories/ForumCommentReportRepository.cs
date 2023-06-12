using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.Repositories
{
    public class ForumCommentReportRepository : IForumCommentReportRepository
    {
        private ForumCommentReportFileHandler _fileHandler;
        private List<ForumCommentReport> _reports;

        public ForumCommentReportRepository()
        {
            _fileHandler = new ForumCommentReportFileHandler();
            Load();
        }

        private void Load()
        {
            _reports = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_reports);
        }

        private int GenerateId()
        {
            return _reports.Count == 0 ? 1 : _reports[_reports.Count - 1].Id + 1;
        }

        public ForumCommentReport GetById(int id)
        {
            return _reports.Find(r => r.Id == id);
        }

        public ForumCommentReport GetByOwnerIdAndCommentId(int ownerId, int commentId)
        {
            return _reports.Find(r => r.OwnerId==ownerId && r.ForumCommentId == commentId);
        }

        

        public List<ForumCommentReport> GetAll()
        {
            return _reports;
        }

        public void Add(ForumCommentReport report)
        {
            report.Id = GenerateId();
            _reports.Add(report);
            Save();
        }

    }
}

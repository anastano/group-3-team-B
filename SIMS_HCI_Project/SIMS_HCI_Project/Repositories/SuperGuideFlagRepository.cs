using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    internal class SuperGuideFlagRepository : ISuperGuideFlagRepository
    {
        private SuperGuideFlagFileHandler _fileHandler;
        private static List<SuperGuideFlag> _flags;

        public SuperGuideFlagRepository()
        {
            _fileHandler = new SuperGuideFlagFileHandler();
            if (_flags == null)
            {
                Load();
            }
        }

        private void Load()
        {
            _flags = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_flags);
        }

        private int GenerateId()
        {
            return _flags.Count == 0 ? 1 : _flags[_flags.Count - 1].Id + 1;
        }

        public SuperGuideFlag GetById(int id)
        {
            return _flags.Find(f => f.Id == id);
        }

        public List<SuperGuideFlag> GetValidByGuide(int guideId)
        {
            return _flags.FindAll(f => f.GuideId == guideId && f.ExpiryDate > DateTime.Now);
        }

        public void Add(SuperGuideFlag flag)
        {
            flag.Id = GenerateId();
            _flags.Add(flag);
            Save();
        }

        public void Update(SuperGuideFlag flag)
        {
            SuperGuideFlag flagToUpdate = GetById(flag.Id);
            flagToUpdate = flag;

            Save();
        }
    }
}

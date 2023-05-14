using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class SuperGuestTitleFileHandler
    {
        private const string FilePath = "../../../Resources/Database/superGuestTitles.csv";

        private readonly Serializer<SuperGuestTitle> _serializer;

        public SuperGuestTitleFileHandler()
        {
            _serializer = new Serializer<SuperGuestTitle>();
        }

        public List<SuperGuestTitle> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<SuperGuestTitle> titles)
        {
            _serializer.ToCSV(FilePath, titles);
        }
    }
}

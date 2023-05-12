using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RenovationFileHandler
    {
        private const string path = "../../../Resources/Database/renovations.csv";

        private readonly Serializer<Renovation> _serializer;

        public RenovationFileHandler()
        {
            _serializer = new Serializer<Renovation>();
        }

        public List<Renovation> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<Renovation> renovations)
        {
            _serializer.ToCSV(path, renovations);
        }
    }
}

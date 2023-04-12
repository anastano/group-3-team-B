using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class OwnerFileHandler
    {
        private const string path = "../../../Resources/Database/owners.csv";

        private readonly Serializer<Owner> _serializer;

        public OwnerFileHandler()
        {
            _serializer = new Serializer<Owner>();
        }


        public List<Owner> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<Owner> owners)
        {
            _serializer.ToCSV(path, owners);
        }
    }
}

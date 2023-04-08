using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class Guest1FileHandler
    {
        private const string path = "../../../Resources/Database/guest1.csv";

        private readonly Serializer<Guest1> _serializer;

        public Guest1FileHandler()
        {
            _serializer = new Serializer<Guest1>();
        }

        public List<Guest1> Load()
        {
            return _serializer.FromCSV(path);
        }

        public void Save(List<Guest1> guests)
        {
            _serializer.ToCSV(path, guests);
        }
    }
}

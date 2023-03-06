using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class AccommodationFileHandler
    {
        private const string path = "../../../Resources/Database/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;


        public AccommodationFileHandler()
        {
            _serializer = new Serializer<Accommodation>();
        }

        public List<Accommodation> Load()
        {
            return _serializer.FromCSV(path);
        }
        
        public void Save(List <Accommodation> accommodations) 
        {
            _serializer.ToCSV(path, accommodations);
        }
    }
}

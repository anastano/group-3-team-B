using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RegularTourRequestFileHandler
    {
        private const string FilePath = "../../../Resources/Database/regularTourRequests.csv";

        private readonly Serializer<RegularTourRequest> _serializer;

        public RegularTourRequestFileHandler()
        {
            _serializer = new Serializer<RegularTourRequest>();
        }

        public List<RegularTourRequest> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<RegularTourRequest> requests)
        {
            _serializer.ToCSV(FilePath, requests);
        }
    }
}

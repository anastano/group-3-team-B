using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RescheduleRequestFileHandler
    {
        private const string path = "../../../Resources/Database/rescheduleRequests.csv";

        private readonly Serializer<RescheduleRequest> _serializer;
        public RescheduleRequestFileHandler()
        {
            _serializer = new Serializer<RescheduleRequest>();
        }

        public List<RescheduleRequest> Load()
        {
            return _serializer.FromCSV(path);
        }
        public void Save(List<RescheduleRequest> requests)
        {
            _serializer.ToCSV(path, requests);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.FileHandlers
{
    public class GuestTourAttendanceFileHandler
    {
        private const string FilePath = "../../../Resources/Database/guesttourattendances.csv";

        private readonly Serializer<GuestTourAttendance> _serializer;

        public GuestTourAttendanceFileHandler()
        {
            _serializer = new Serializer<GuestTourAttendance>();
        }

        public List<GuestTourAttendance> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<GuestTourAttendance> guestTourAttendances)
        {
            _serializer.ToCSV(FilePath, guestTourAttendances);
        }
    }
}

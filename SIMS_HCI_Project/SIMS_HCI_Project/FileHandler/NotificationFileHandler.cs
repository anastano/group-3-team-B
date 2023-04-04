using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandler
{
    public class NotificationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/notifications.csv";

        private readonly Serializer<Notification> _serializer;

        public NotificationFileHandler()
        {
            _serializer = new Serializer<Notification>();
        }

        public List<Notification> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<Notification> notifications)
        {
            _serializer.ToCSV(FilePath, notifications);
        }
    }
}

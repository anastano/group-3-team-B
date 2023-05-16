using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class NotificationFileHandler
    {
        private const string FilePath = "../../../Resources/Database/notifications.csv";
        private const char Delimiter = '|';

        public NotificationFileHandler()
        {
        }

        public List<Notification> Load()
        {
            List<Notification> notifications = new List<Notification>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                Notification notification = new Notification();

                notification.Id = int.Parse(csvValues[0]);
                notification.Message = csvValues[1];
                notification.UserId = int.Parse(csvValues[2]);
                notification.IsRead = bool.Parse(csvValues[3]);

                notifications.Add(notification);
            }

            return notifications;
        }

        public void Save(List<Notification> notifications)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Notification notification in notifications)
            {
                string[] csvValues =
                {
                    notification.Id.ToString(),
                    notification.Message.ToString(),
                    notification.UserId.ToString(),
                    notification.IsRead.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

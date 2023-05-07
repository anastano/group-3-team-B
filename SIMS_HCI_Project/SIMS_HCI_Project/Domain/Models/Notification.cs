using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum NotificationType { DEFAULT, TOUR_REQUEST_ACCEPTED}

    public class Notification : ISerializable
    {
        public int Id { get; set; }
        public String Message { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }

        public Notification()
        {
            Message = " ";
            IsRead = false;
            Type = NotificationType.DEFAULT;
        }

        public Notification(string message, int userId, bool isRead, NotificationType type = NotificationType.DEFAULT)
        {
            Message = message;
            UserId = userId;
            IsRead = isRead;
            Type = type;
        }

        public Notification(Notification notification)
        {
            Id = notification.Id;
            Message = notification.Message;
            UserId = notification.UserId;
            User = notification.User;
            IsRead = notification.IsRead;
            Type = notification.Type;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Message.ToString(),
                UserId.ToString(),
                IsRead.ToString(),
                Type.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Message = values[1];
            UserId = int.Parse(values[2]);
            IsRead = bool.Parse(values[3]);
            Enum.TryParse(values[4], out NotificationType type);
            Type = type;
        }
    }
}

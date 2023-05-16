using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum NotificationType { DEFAULT, TOUR_REQUEST_ACCEPTED, NEW_TOUR, CONFIRM_ATTENDANCE}

    public class Notification
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

        public int ExtractTourId(Notification notification) //add to class diagram
        {
            var regex = new Regex(@"\[(\d+)\]");
            var match = regex.Match(notification.Message);
            if (match.Success)
            {
                var tourIdString = match.Groups[1].Value;
                if (int.TryParse(tourIdString, out int tourId))
                {
                    return tourId;
                }
            }
            return 0;
        }
    }
}

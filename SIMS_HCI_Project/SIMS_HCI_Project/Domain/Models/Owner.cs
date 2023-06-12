using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Owner : User
    {
        public double AverageRating { get; set; }
        public bool SuperFlag { get; set; }

        public Owner() 
        {
            AverageRating = 0;
            SuperFlag = false;
        }

        public Owner(User user) 
        {
            Id = user.Id;
            Password = user.Password;
            Username = user.Username;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Role = user.Role;
            AccountActive = user.AccountActive;
        }
    }
}

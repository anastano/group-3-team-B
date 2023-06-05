using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Guest2 : User
    {
        public Guest2()
        {
        }

        public Guest2(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Role = user.Role;
            AccountActive = user.AccountActive;
        }
    }
}

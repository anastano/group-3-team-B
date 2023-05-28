
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum UserRole { OWNER, GUEST1, GUIDE, GUEST2 };

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public User() { }

        public User(int id, string username, string password, UserRole role, string name, string surname, int age)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
            Name = name;
            Surname = surname;
            Age = age;
        }
        public string GetFullName()
        {
            return Name + " " + Surname;
        }
    }
}

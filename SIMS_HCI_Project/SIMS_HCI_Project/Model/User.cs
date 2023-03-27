using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public enum UserRole { OWNER, GUEST1, GUIDE, GUEST2 };

    public class User : ISerializable
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

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString(), Name, Surname, Age.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Enum.TryParse(values[3], out UserRole role);
            Role = role;
            Name = values[4];
            Surname = values[5];
            Age = Convert.ToInt32(values[6]);
        }

    }
}

using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    // [Maybe]TODO: Owner, Guests and Guide classes should inherit User class 
    public class User : ISerializable
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id, Username, Password };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = values[0];
            Username = values[1];
            Password = values[2];
        }

    }
}

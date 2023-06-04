using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SIMS_HCI_Project.FileHandlers
{
    public class UserFileHandler
    {
        private const string FilePath = "../../../Resources/Database/users.csv";
        private const char Delimiter = '|';

        public UserFileHandler() { }

        public List<User> Load()
        {
            List<User> users = new List<User>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                User user = new User();

                user.Id = Convert.ToInt32(csvValues[0]);
                user.Username = csvValues[1];
                user.Password = csvValues[2];
                Enum.TryParse(csvValues[3], out UserRole role);
                user.Role = role;
                user.Name = csvValues[4];
                user.Surname = csvValues[5];
                user.Age = Convert.ToInt32(csvValues[6]);
                user.ActiveAccount = Convert.ToBoolean(csvValues[7]);

                users.Add(user);
            }

            return users;
        }

        public void Save(List<User> users)
        {
            StringBuilder csv = new StringBuilder();

            foreach (User user in users)
            {
                string[] csvValues =
                {
                    user.Id.ToString(),
                    user.Username,
                    user.Password,
                    user.Role.ToString(),
                    user.Name,
                    user.Surname,
                    user.Age.ToString(),
                    user.ActiveAccount.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

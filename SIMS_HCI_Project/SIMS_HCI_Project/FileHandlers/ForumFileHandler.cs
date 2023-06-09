using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class ForumFileHandler
    {
        private const string FilePath = "../../../Resources/Database/forums.csv";
        private const char Delimiter = '|';

        public ForumFileHandler() { }

        public List<Forum> Load()
        {
            List<Forum> forums = new List<Forum>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                Forum forum = new Forum();

                forum.Id = Convert.ToInt32(csvValues[0]);
                forum.UserId = Convert.ToInt32(csvValues[1]);
                forum.LocationId = Convert.ToInt32(csvValues[2]);
                Enum.TryParse(csvValues[3], out ForumStatus status);
                forum.Status = status;

                forums.Add(forum);
            }

            return forums;
        }

        public void Save(List<Forum> forums)
        {
            StringBuilder csv = new StringBuilder();

            foreach (Forum forum in forums)
            {
                string[] csvValues =
                {
                    forum.Id.ToString(),
                    forum.UserId.ToString(),
                    forum.LocationId.ToString(),
                    forum.Status.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

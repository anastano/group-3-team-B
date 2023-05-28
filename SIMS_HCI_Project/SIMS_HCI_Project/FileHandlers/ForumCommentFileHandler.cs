using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class ForumCommentFileHandler
    {
        private const string FilePath = "../../../Resources/Database/forumComments.csv";
        private const char Delimiter = '|';

        public ForumCommentFileHandler() { }

        public List<ForumComment> Load()
        {
            List<ForumComment> comments = new List<ForumComment>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                ForumComment comment = new ForumComment();

                comment.Id = Convert.ToInt32(csvValues[0]);
                comment.UserId = Convert.ToInt32(csvValues[1]);
                comment.ForumId = Convert.ToInt32(csvValues[2]);
                comment.Content = csvValues[3];
                comment.ReportCounter = Convert.ToInt32(csvValues[4]);

                comments.Add(comment);
            }

            return comments;
        }

        public void Save(List<ForumComment> comments)
        {
            StringBuilder csv = new StringBuilder();

            foreach (ForumComment comment in comments)
            {
                string[] csvValues =
                {
                    comment.Id.ToString(),
                    comment.UserId.ToString(),
                    comment.ForumId.ToString(),
                    comment.Content,
                    comment.ReportCounter.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

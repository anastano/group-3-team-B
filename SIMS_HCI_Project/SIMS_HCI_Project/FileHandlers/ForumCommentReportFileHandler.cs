using ceTe.DynamicPDF.ReportWriter;
using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class ForumCommentReportFileHandler
    {
        private const string FilePath = "../../../Resources/Database/forumCommentReports.csv";
        private const char Delimiter = '|';

        public ForumCommentReportFileHandler() { }

        public List<ForumCommentReport> Load()
        {
            List<ForumCommentReport> reports = new List<ForumCommentReport>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                ForumCommentReport report = new ForumCommentReport();

                report.Id = Convert.ToInt32(csvValues[0]);
                report.OwnerId = Convert.ToInt32(csvValues[1]);
                report.ForumCommentId = Convert.ToInt32(csvValues[2]);

                reports.Add(report);
            }

            return reports;
        }

        public void Save(List<ForumCommentReport> reports)
        {
            StringBuilder csv = new StringBuilder();

            foreach (ForumCommentReport report in reports)
            {
                string[] csvValues =
                {
                    report.Id.ToString(),
                    report.OwnerId.ToString(),
                    report.ForumCommentId.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

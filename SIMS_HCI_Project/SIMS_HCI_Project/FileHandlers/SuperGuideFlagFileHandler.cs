using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    internal class SuperGuideFlagFileHandler
    {
        private const string FilePath = "../../../Resources/Database/superGuideFlags.csv";
        private const char Delimiter = '|';

        public SuperGuideFlagFileHandler()
        {

        }

        public List<SuperGuideFlag> Load()
        {
            List<SuperGuideFlag> flags = new List<SuperGuideFlag>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                SuperGuideFlag flag = new SuperGuideFlag();

                flag.Id = Convert.ToInt32(csvValues[0]);
                flag.GuideId = Convert.ToInt32(csvValues[1]);
                flag.Language = csvValues[2];
                flag.AcquiredDate = DateTime.ParseExact(csvValues[3], "M/d/yyyy h:mm:ss tt", null);
                flag.ExpiryDate = DateTime.ParseExact(csvValues[4], "M/d/yyyy h:mm:ss tt", null);

                flags.Add(flag);
            }

            return flags;
        }

        public void Save(List<SuperGuideFlag> flags)
        {
            StringBuilder csv = new StringBuilder();

            foreach (SuperGuideFlag flag in flags)
            {
                string[] csvValues =
                {
                    flag.Id.ToString(),
                    flag.GuideId.ToString(),
                    flag.Language,
                    flag.AcquiredDate.ToString("M/d/yyyy h:mm:ss tt"),
                    flag.ExpiryDate.ToString("M/d/yyyy h:mm:ss tt")
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

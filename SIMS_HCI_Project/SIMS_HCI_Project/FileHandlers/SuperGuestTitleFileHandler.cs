using SIMS_HCI_Project.Domain.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization;

namespace SIMS_HCI_Project.FileHandlers
{
    public class SuperGuestTitleFileHandler
    {
        private const string FilePath = "../../../Resources/Database/superGuestTitles.csv";
        private const char Delimiter = '|';

        public SuperGuestTitleFileHandler()
        {

        }

        public List<SuperGuestTitle> Load()
        { 
            List<SuperGuestTitle> titles = new List<SuperGuestTitle>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                SuperGuestTitle title = new SuperGuestTitle();

                title.Id = int.Parse(csvValues[0]);
                title.GuestId = int.Parse(csvValues[1]);
                title.ActivationDate = DateTime.ParseExact(csvValues[2], "MM/dd/yyyy", null);
                title.AvailablePoints = int.Parse(csvValues[3]);
                Enum.TryParse(csvValues[4], out TitleStatus status);
                title.Status = status;

                titles.Add(title);
            }

            return titles;
        }

        public void Save(List<SuperGuestTitle> titles)
        {
            StringBuilder csv = new StringBuilder();

            foreach (SuperGuestTitle title in titles)
            {
                string[] csvValues =
                {
                    title.Id.ToString(),
                    title.GuestId.ToString(),
                    title.ActivationDate.ToString("MM/dd/yyyy"),
                    title.AvailablePoints.ToString(),
                    title.Status.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class ComplexTourRequestFileHandler
    {
        private const string FilePath = "../../../Resources/Database/complexTourRequests.csv";
        private const char Delimiter = '|';

        public ComplexTourRequestFileHandler()
        {

        }

        public List<ComplexTourRequest> Load()
        {
            List<ComplexTourRequest> requests = new List<ComplexTourRequest>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                ComplexTourRequest request = new ComplexTourRequest();

                request.Id = int.Parse(csvValues[0]);
                Enum.TryParse(csvValues[1], out TourRequestStatus status);
                request.Status = status;
                request.GuestId = int.Parse(csvValues[2]);

                requests.Add(request);
            }

            return requests;
        }

        public void Save(List<ComplexTourRequest> requests)
        {
            StringBuilder csv = new StringBuilder();

            foreach (ComplexTourRequest request in requests)
            {
                string[] csvValues =
                {
                    request.Id.ToString(),
                    request.Status.ToString(),
                    request.GuestId.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

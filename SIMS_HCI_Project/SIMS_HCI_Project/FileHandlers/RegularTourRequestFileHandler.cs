using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;


namespace SIMS_HCI_Project.FileHandlers
{
    public class RegularTourRequestFileHandler
    {
        private const string FilePath = "../../../Resources/Database/regularTourRequests.csv";
        private const char Delimiter = '|';

        public RegularTourRequestFileHandler()
        {

        }

        public List<RegularTourRequest> Load()
        {
            List<RegularTourRequest> requests = new List<RegularTourRequest>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                RegularTourRequest request = new RegularTourRequest();

                request.Id = int.Parse(csvValues[0]);
                Enum.TryParse(csvValues[1], out TourRequestStatus status);
                request.Status = status;
                request.GuestId = int.Parse(csvValues[2]);
                request.LocationId = int.Parse(csvValues[3]);
                request.Language = csvValues[4];
                request.GuestNumber = int.Parse(csvValues[5]);
                request.Description = csvValues[6];
                request.DateRange.Start = DateTime.ParseExact(csvValues[7], "MM/dd/yyyy", null);
                request.DateRange.End = DateTime.ParseExact(csvValues[8], "MM/dd/yyyy", null);
                request.SubmittingDate = DateTime.ParseExact(csvValues[9], "MM/dd/yyyy", null);
                request.ComplexTourRequestId = int.Parse(csvValues[10]);
                request.TourId = int.Parse(csvValues[11]);

                requests.Add(request);
            }

            return requests;
        }

        public void Save(List<RegularTourRequest> requests)
        {
            StringBuilder csv = new StringBuilder();

            foreach (RegularTourRequest request in requests)
            {
                string[] csvValues =
                {
                    request.Id.ToString(),
                    request.Status.ToString(),
                    request.GuestId.ToString(),
                    request.LocationId.ToString(),
                    request.Language.ToString(),
                    request.GuestNumber.ToString(),
                    request.Description.ToString(),
                    request.DateRange.Start.ToString("MM/dd/yyyy"),
                    request.DateRange.End.ToString("MM/dd/yyyy"),
                    request.SubmittingDate.ToString("MM/dd/yyyy"),
                    request.ComplexTourRequestId.ToString(),
                    request.TourId.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

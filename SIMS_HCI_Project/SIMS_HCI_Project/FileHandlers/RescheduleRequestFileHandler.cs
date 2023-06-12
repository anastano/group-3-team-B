using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIMS_HCI_Project.FileHandlers
{
    public class RescheduleRequestFileHandler
    {
        private const string path = "../../../Resources/Database/rescheduleRequests.csv";
        private const char Delimiter = '|';
        public RescheduleRequestFileHandler() {}
        public List<RescheduleRequest> Load()
        {
            List<RescheduleRequest> requests = new List<RescheduleRequest>();

            foreach (string line in File.ReadLines(path))
            {
                string[] csvValues = line.Split(Delimiter);
                RescheduleRequest request = new RescheduleRequest();

                request.Id = int.Parse(csvValues[0]);
                request.AccommodationReservationId = int.Parse(csvValues[1]);
                request.WantedStart = DateTime.ParseExact(csvValues[2], "MM/dd/yyyy", null);
                request.WantedEnd = DateTime.ParseExact(csvValues[3], "MM/dd/yyyy", null);
                Enum.TryParse(csvValues[4], out RescheduleRequestStatus status);
                request.Status = status;
                request.OwnerComment = csvValues[5];

                requests.Add(request);
            }

            return requests;
        }

        public void Save(List<RescheduleRequest> requests)
        {
            StringBuilder csv = new StringBuilder();

            foreach (RescheduleRequest request in requests)
            {
                string[] csvValues =
                {
                    request.Id.ToString(),
                    request.AccommodationReservationId.ToString(),
                    request.WantedStart.ToString("MM/dd/yyyy"),
                    request.WantedEnd.ToString("MM/dd/yyyy"),
                    request.Status.ToString(),
                    request.OwnerComment
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}

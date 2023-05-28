using System;
﻿using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SIMS_HCI_Project.FileHandlers
{
    public class GuestTourAttendanceFileHandler
    {
        private const string FilePath = "../../../Resources/Database/guesttourattendances.csv";
        private const char Delimiter = '|';

        public GuestTourAttendanceFileHandler() { }

        public List<GuestTourAttendance> Load()
        {
            List<GuestTourAttendance> guestTourAttendances = new List<GuestTourAttendance>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                GuestTourAttendance guestTourAttendance = new GuestTourAttendance();

                guestTourAttendance.Id = Convert.ToInt32(csvValues[0]);
                guestTourAttendance.TourReservationId = Convert.ToInt32(csvValues[1]);
                Enum.TryParse(csvValues[2], out AttendanceStatus status);
                guestTourAttendance.Status = status;
                guestTourAttendance.KeyPointJoinedId = Convert.ToInt32(csvValues[3]);

                guestTourAttendances.Add(guestTourAttendance);
            }

            return guestTourAttendances;
        }

        public void Save(List<GuestTourAttendance> guestTourAttendances)
        {
            StringBuilder csv = new StringBuilder();

            foreach (GuestTourAttendance guestTourAttendance in guestTourAttendances)
            {
                string[] csvValues =
                {
                    guestTourAttendance.Id.ToString(),
                    guestTourAttendance.TourReservation.Id.ToString(),
                    guestTourAttendance.Status.ToString(),
                    guestTourAttendance.KeyPointJoinedId.ToString()
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourVoucherFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourVouchers.csv";
        private const char Delimiter = '|';

        public TourVoucherFileHandler() { }

        public List<TourVoucher> Load()
        {
            List<TourVoucher> tourVouchers = new List<TourVoucher>();

            foreach (string line in File.ReadLines(FilePath))
            {
                string[] csvValues = line.Split(Delimiter);
                TourVoucher tourVoucher = new TourVoucher();

                tourVoucher.Id = Convert.ToInt32(csvValues[0]);
                tourVoucher.Title = csvValues[1];
                tourVoucher.GuestId = Convert.ToInt32(csvValues[2]);
                tourVoucher.AquiredDate = DateTime.ParseExact(csvValues[3], "M/d/yyyy h:mm:ss tt", null);
                tourVoucher.ExpirationDate = DateTime.ParseExact(csvValues[4], "M/d/yyyy h:mm:ss tt", null);
                Enum.TryParse(csvValues[5], out VoucherStatus status);
                tourVoucher.Status = status;

                tourVouchers.Add(tourVoucher);
            }

            return tourVouchers;
        }

        public void Save(List<TourVoucher> tourVouchers)
        {
            StringBuilder csv = new StringBuilder();

            foreach (TourVoucher tourVoucher in tourVouchers)
            {
                string[] csvValues =
                {
                    tourVoucher.Id.ToString(),
                    tourVoucher.Title,
                    tourVoucher.GuestId.ToString(),
                    tourVoucher.AquiredDate.ToString("M/d/yyyy h:mm:ss tt"),
                    tourVoucher.ExpirationDate.ToString("M/d/yyyy h:mm:ss tt"),
                    tourVoucher.Status.ToString() 
                };

                string line = string.Join(Delimiter.ToString(), csvValues);
                csv.AppendLine(line);
            }

            File.WriteAllText(FilePath, csv.ToString());
        }
    }
}

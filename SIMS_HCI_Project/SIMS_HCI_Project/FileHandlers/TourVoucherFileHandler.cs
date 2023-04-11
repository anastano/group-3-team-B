using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.FileHandlers
{
    public class TourVoucherFileHandler
    {
        private const string FilePath = "../../../Resources/Database/tourVouchers.csv";

        private readonly Serializer<TourVoucher> _serializer;

        public TourVoucherFileHandler()
        {
            _serializer = new Serializer<TourVoucher>();
        }

        public List<TourVoucher> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<TourVoucher> tourVouchers)
        {
            _serializer.ToCSV(FilePath, tourVouchers);
        }
    }
}

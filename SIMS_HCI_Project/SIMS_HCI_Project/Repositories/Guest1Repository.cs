using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class Guest1Repository : IGuest1Repository
    {
        private readonly Guest1FileHandler _fileHandler;

        private static List<Guest1> _guests;

        public Guest1Repository()
        {
            _fileHandler = new Guest1FileHandler();
            _guests = _fileHandler.Load();
        }

        public Guest1 GetById(int id)
        {
            return _guests.Find(g => g.Id == id);
        }

        public List<Guest1> GetAll()
        {
            return _guests;
        }


    }
}

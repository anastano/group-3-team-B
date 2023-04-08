using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class Guest1Service
    {
        private readonly IGuest1Repository _guest1Repository;

        public Guest1Service()
        {
            _guest1Repository = Injector.Injector.CreateInstance<IGuest1Repository>();
        }

        public Guest1 FindById(int id)
        {
            return _guest1Repository.FindById(id);
        }

        public List<Guest1> GetAll()
        {
            return _guest1Repository.GetAll();
        }

        public void Load()
        {
            _guest1Repository.Load();
        }

        public void Save()
        {
            _guest1Repository.Save();
        }


    }
}

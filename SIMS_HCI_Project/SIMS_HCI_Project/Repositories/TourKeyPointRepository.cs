﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;

namespace SIMS_HCI_Project.Repositories
{
    public class TourKeyPointRepository : ITourKeyPointRepository
    {
        private TourKeyPointFileHandler _fileHandler;
        private static List<TourKeyPoint> _tourKeyPoints;

        public TourKeyPointRepository()
        {
            _fileHandler = new TourKeyPointFileHandler();
            if(_tourKeyPoints == null)
            {
                Load();
            }
        }

        public void Load()
        {
            _tourKeyPoints = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_tourKeyPoints);
        }

        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPoints;
        }

        public TourKeyPoint FindById(int id) //preimenuj u get
        {
            return _tourKeyPoints.Find(tkp => tkp.Id == id);
        }

        public List<TourKeyPoint> FindByIds(List<int> ids) //preimenuj u get
        {
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();
            foreach (int id in ids)
            {
                tourKeyPoints.Add(FindById(id));
            }

            return tourKeyPoints;
        }
    }
}
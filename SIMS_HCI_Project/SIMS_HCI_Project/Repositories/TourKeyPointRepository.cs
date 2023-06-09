﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Load()
        {
            _tourKeyPoints = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_tourKeyPoints);
        }

        private int GenerateId()
        {
            return _tourKeyPoints.Count == 0 ? 1 : _tourKeyPoints[_tourKeyPoints.Count - 1].Id + 1;
        }

        public TourKeyPoint GetById(int id)
        {
            return _tourKeyPoints.Find(tkp => tkp.Id == id);
        }

        public List<TourKeyPoint> GetByIds(List<int> ids) // make one liner
        {
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();
            foreach (int id in ids)
            {
                tourKeyPoints.Add(GetById(id));
            }

            return tourKeyPoints;
        }
        
        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPoints;
        }

        public void Add(TourKeyPoint tourKeyPoint)
        {
            tourKeyPoint.Id = GenerateId();
            _tourKeyPoints.Add(tourKeyPoint);
            Save();
        }

        public void AddBulk(List<TourKeyPoint> tourKeyPoints)
        {
            foreach (TourKeyPoint tourKeyPoint in tourKeyPoints)
            {
                tourKeyPoint.Id = GenerateId();
                _tourKeyPoints.Add(tourKeyPoint);
            }
            Save();
        }
    }
}

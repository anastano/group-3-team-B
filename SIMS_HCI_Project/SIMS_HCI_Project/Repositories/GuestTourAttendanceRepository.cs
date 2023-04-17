using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Applications.Services;
﻿using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.DTOs;


namespace SIMS_HCI_Project.Repositories
{
    public class GuestTourAttendanceRepository : IGuestTourAttendanceRepository
    {
        private GuestTourAttendanceFileHandler _fileHandler;
        private static List<GuestTourAttendance> _guestTourAttendances;

        public GuestTourAttendanceRepository()
        {
            _fileHandler = new GuestTourAttendanceFileHandler();

            if (_guestTourAttendances == null)
            {
                Load();
            }
        }
        public void Load()
        {
            _guestTourAttendances = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_guestTourAttendances);
        }

        public void Add(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Id = GenerateId();
            _guestTourAttendances.Add(guestTourAttendance);
            Save();
        }

        public int GenerateId()
        {
            return _guestTourAttendances.Count == 0 ? 1 : _guestTourAttendances[_guestTourAttendances.Count - 1].Id + 1;
        }
        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendances;
        }

        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendances.FindAll(gta => gta.TourTimeId == id);
        }
        
        public GuestTourAttendance GetById(int id)
        {
            return _guestTourAttendances.Find(gta => gta.Id == id);
        }

        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId)
        {
            return _guestTourAttendances.Find(g => g.GuestId == guestId && g.TourTimeId == tourTimeId);
        }

        public List<GuestTourAttendance> GetAllByGuestId( int guestId)
        {
            return _guestTourAttendances.FindAll(g => g.GuestId == guestId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId, TourTimeService tourTimeService) // TODO izbaci service
        {
            List<TourTime> tourTimes = new List<TourTime>();
            foreach(var gta in GetAllByGuestId(guestId))
            {
                if(gta.Status == AttendanceStatus.PRESENT)
                {
                    tourTimes.Add(tourTimeService.GetById(gta.TourTimeId));
                }
            }
            return tourTimes;
        }
        public List<GuestTourAttendance> GetByConfirmationRequestedStatus(int guestId)
        {
            var result = new List<GuestTourAttendance>();
            foreach(var gta in GetAllByGuestId(guestId))
            {
                if(gta.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
                {
                    result.Add(gta);
                }
            }
            return result;
        }

        public int GetGuestCountByAgeGroup(AgeGroup ageGroup, int tourTimeId)
        {
            return _guestTourAttendances.FindAll(gta => gta.Guest.Age >= ageGroup.MinAge && gta.Guest.Age <= ageGroup.MaxAge && gta.TourTimeId == tourTimeId).Count;
        }

        public TourTime GetTourWithMostGuests()
        {
            return _guestTourAttendances.Where(gta => gta.TourTime.Status == TourStatus.COMPLETED).GroupBy(gta => gta.TourTimeId).OrderByDescending(gta => gta.Count()).First().First().TourTime;
        }

        public TourTime GetTourWithMostGuestsByYear(int year)
        {
            return _guestTourAttendances.Where(gta => gta.TourTime.DepartureTime.Year == year && gta.TourTime.Status == TourStatus.COMPLETED).ToList().GroupBy(gta => gta.TourTimeId).OrderByDescending(gta => gta.Count()).First().First().TourTime;
        }

        public int GetGuestsWithVoucherCount(int tourTimeId)
        {
            return _guestTourAttendances.Where(gta => gta.TourReservation.VoucherUsedId != -1 && gta.TourTimeId == tourTimeId).Count();
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            GuestTourAttendance attendance = GetByGuestAndTourTimeIds(guestId, tourTimeId);
            return _guestTourAttendances.Any(gta => gta.Status == AttendanceStatus.PRESENT);
        }
        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId)
        {
            GuestTourAttendance attendance = GetByGuestAndTourTimeIds(guestId, tourTimeId);
            if (attendance.Status == AttendanceStatus.CONFIRMATION_REQUESTED)
            {
                attendance.Status = AttendanceStatus.PRESENT;
                Save();
            }
        }
    }
}

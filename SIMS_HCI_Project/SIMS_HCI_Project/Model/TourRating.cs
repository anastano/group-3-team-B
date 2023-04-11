﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Serializer;


namespace SIMS_HCI_Project.Model
{
    public class TourRating : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public int GuideId { get; set; }
        public TourReservation TourReservation { get; set; }
        public int OverallExperience { get; set; }
        public int Organisation { get; set; }
        public int Interestingness { get; set; }
        public int GuidesKnowledge { get; set; }
        public int GuidesLanguage { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }

        public TourRating()
        {
        }

        public TourRating(int id, int guestId, int reservationId, int guideId, TourReservation tourReservation, int overallExperience, int organisation, int interestingness, int guidesKnowledge, int guidesLanguage, string comment, List<string> images)
        {
            Id = id;
            GuestId = guestId;
            ReservationId = reservationId;
            GuideId = guideId;
            OverallExperience = overallExperience;
            Organisation = organisation;
            Interestingness = interestingness;
            GuidesKnowledge = guidesKnowledge;
            GuidesLanguage = guidesLanguage;
            Comment = comment;
            Images = images;

            TourReservation = new TourReservation();
            Images = new List<string>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                ReservationId.ToString(),
                GuideId.ToString(),
                OverallExperience.ToString(),
                Organisation.ToString(),
                Interestingness.ToString(),
                GuidesKnowledge.ToString(),
                GuidesLanguage.ToString(),
                Comment,
                string.Join(",", Images)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            ReservationId = int.Parse(values[2]);
            GuideId = int.Parse(values[3]);
            OverallExperience = int.Parse(values[4]);
            Organisation = int.Parse(values[5]);
            Interestingness = int.Parse(values[6]);
            GuidesKnowledge = int.Parse(values[7]);
            GuidesLanguage = int.Parse(values[8]);
            Comment = values[9];
            Images = new List<string>(values[10].Split(","));
        }
    }
}

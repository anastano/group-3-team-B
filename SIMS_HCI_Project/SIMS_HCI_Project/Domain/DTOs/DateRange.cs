using SIMS_HCI_Project.Applications.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange() { }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateRange(DateTime start, int durationHrs)
        {
            Start = start;
            End = start.AddHours(durationHrs);
        }

        /* DateRange */
        public bool DoesOverlap(DateRange dateRange)
        {
            return !(this.IsAfter(dateRange) || this.IsBefore(dateRange));
        }
        public bool IsBefore(DateRange dateRange)
        {
            return this.Start < dateRange.Start && this.End < dateRange.Start;
        }

        public bool IsAfter(DateRange dateRange)
        {
            return this.Start > dateRange.End && this.End > dateRange.End;
        }

        public bool IsInside(DateRange dateRange)
        {
            return this.Start > dateRange.Start && this.End < dateRange.End;
        }

        public bool HasInside(DateRange dateRange)
        {
            return this.Start < dateRange.Start && this.End > dateRange.End;
        }
        
        /* DateTime comparison */
        public bool IsBefore(DateTime dateTime)
        {
            return this.Start < dateTime && this.End < dateTime;
        }

        public bool IsAfter(DateTime dateTime)
        {
            return this.Start > dateTime && this.End > dateTime;
        }

        public bool HasInside(DateTime dateTime)
        {
            return dateTime > this.Start && dateTime < this.End;
        }
        //jedino nam se razlikuju po znaku znaku jednako provjeri je li mozes staviti jednako
        public bool IsInProgress()
        {
            return DateTime.Today >= this.Start && DateTime.Today <= this.End;
        }
    }
}

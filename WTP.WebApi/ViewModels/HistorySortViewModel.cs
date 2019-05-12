using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public enum HistorySortState
    {
        NameAsc,    // by Name Ascending
        NameDesc,   // by Name Descending
        EmailAsc,   // by Email Ascending
        EmailDesc,  // by Email Descending
        IdAsc,      // by Id Ascending
        IdDesc,     // by Id Descending
        UserIdAsc,
        UserIdDesc,
        AdminIdAsc,
        AdminIdDesc,
        DateAsc,
        DateDesc
    }

    public class HistorySortViewModel
    {
        public HistorySortState NameSort { get; private set; } // value for sorting by name
        public HistorySortState EmailSort { get; private set; } // value for sorting by email
        public HistorySortState IdSort { get; private set; } // value for sorting by id
        public HistorySortState UserIdSort { get; private set; }
        public HistorySortState AdminIdSort { get; private set; }
        public HistorySortState DateSort { get; private set; }
        public HistorySortState Current { get; private set; } // current value of sorting

        public HistorySortViewModel(HistorySortState sortOrder)
        {
            NameSort = sortOrder == HistorySortState.NameAsc ? HistorySortState.NameDesc : HistorySortState.NameAsc;
            EmailSort = sortOrder == HistorySortState.EmailAsc ? HistorySortState.EmailDesc : HistorySortState.EmailAsc;
            //UserSort = sortOrder == HistorySortState.UserAsc ? HistorySortState.UserDesc : HistorySortState.UserAsc;
            IdSort = sortOrder == HistorySortState.IdAsc ? HistorySortState.IdDesc : HistorySortState.IdAsc;
            UserIdSort = sortOrder == HistorySortState.UserIdAsc ? HistorySortState.UserIdDesc : HistorySortState.UserIdAsc;
            IdSort = sortOrder == HistorySortState.AdminIdAsc ? HistorySortState.AdminIdDesc : HistorySortState.AdminIdAsc;
            IdSort = sortOrder == HistorySortState.DateAsc ? HistorySortState.DateDesc : HistorySortState.DateAsc;
            Current = sortOrder;
        }
    }
}

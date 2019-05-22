using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.Shared;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class HistorySortDto
    {
        public HistorySortState NameSort { get; private set; } // value for sorting by name
        public HistorySortState EmailSort { get; private set; } // value for sorting by email
        public HistorySortState IdSort { get; private set; } // value for sorting by id
        public HistorySortState UserIdSort { get; private set; }
        public HistorySortState AdminIdSort { get; private set; }
        public HistorySortState DateSort { get; private set; }
        public HistorySortState Current { get; private set; } // current value of sorting

        public HistorySortDto(HistorySortState sortOrder)
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

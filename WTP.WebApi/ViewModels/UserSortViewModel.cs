using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public enum SortState
    {
        NameAsc,    // by Name Ascending
        NameDesc,   // by Name Descending
        EmailAsc,   // by Email Ascending
        EmailDesc,  // by Email Descending
        IdAsc,      // by Id Ascending
        IdDesc      // by Id Descending
        //UserAsc, // 
        //UserDesc // 
    }

    public class UserSortViewModel
    {
        public SortState NameSort { get; private set; } // value for sorting by name
        public SortState EmailSort { get; private set; } // value for sorting by email
        //public SortState UserSort { get; private set; } 
        public SortState IdSort { get; private set; } // value for sorting by id
        public SortState Current { get; private set; } // current value of sorting

        public UserSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;
            //UserSort = sortOrder == SortState.UserAsc ? SortState.UserDesc : SortState.UserAsc;
            IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
            Current = sortOrder;
        }
    }
}

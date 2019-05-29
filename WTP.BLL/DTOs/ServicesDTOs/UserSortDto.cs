using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.Shared;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class UserSortDto
    {
        public SortState NameSort { get; private set; } // value for sorting by name
        public SortState EmailSort { get; private set; } // value for sorting by email
        //public SortState UserSort { get; private set; } 
        public SortState IdSort { get; private set; } // value for sorting by id
        public SortState Current { get; private set; } // current value of sorting

        public UserSortDto(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;
            //UserSort = sortOrder == SortState.UserAsc ? SortState.UserDesc : SortState.UserAsc;
            IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
            Current = sortOrder;
        }


    }
}

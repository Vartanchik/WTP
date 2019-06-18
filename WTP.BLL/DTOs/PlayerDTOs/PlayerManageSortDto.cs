using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.Shared;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerManageSortDto
    {
        public PlayerSortState NameSort { get; private set; } // value for sorting by name
        public PlayerSortState EmailSort { get; private set; } // value for sorting by email
        public PlayerSortState IdSort { get; private set; } // value for sorting by id
        public PlayerSortState UserId { get; private set; } // value for sorting by id
        public PlayerSortState UserName { get; private set; } // value for sorting by id
        public PlayerSortState TeamName { get; private set; } // value for sorting by id
        public PlayerSortState GameName { get; private set; } // value for sorting by id
        public PlayerSortState Current { get; private set; } // current value of sorting

        public PlayerManageSortDto(PlayerSortState sortOrder)
        {
            NameSort = sortOrder == PlayerSortState.NameAsc ? PlayerSortState.NameDesc : PlayerSortState.NameAsc;
            EmailSort = sortOrder == PlayerSortState.EmailAsc ? PlayerSortState.EmailDesc : PlayerSortState.EmailAsc;
            IdSort = sortOrder == PlayerSortState.IdAsc ? PlayerSortState.IdDesc : PlayerSortState.IdAsc;
            UserId = sortOrder == PlayerSortState.UserIdAsc ? PlayerSortState.UserIdDesc : PlayerSortState.UserIdAsc;
            UserName = sortOrder == PlayerSortState.UserNameAsc ? PlayerSortState.UserNameDesc : PlayerSortState.UserNameAsc;
            TeamName = sortOrder == PlayerSortState.TeamNameAsc ? PlayerSortState.TeamNameDesc : PlayerSortState.TeamNameAsc;
            GameName = sortOrder == PlayerSortState.GameNameAsc ? PlayerSortState.GameNameDesc : PlayerSortState.GameNameAsc;
            Current = sortOrder;
        }
    }
}

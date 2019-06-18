using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamControllerInputDto
    {
        public int IdGame { get; set; }
        public int PageSize { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string SortField { get; set; } = "";
        public string SortType { get; set; } = "";
        public string NameValue { get; set; } = "";
        public int WinRateLeftValue { get; set; } = 0;
        public int WinRateRightValue { get; set; } = 100;
        public int MembersLeftValue { get; set; } = 0;
        public int MembersRightValue { get; set; } = 5;
        public GoalDto[] GoalDtos { get; set; } =
        {
            new GoalDto{Id = 1, Name = "fan"},
            new GoalDto{Id = 2, Name = "pro"},
            new GoalDto{Id = 3, Name = "compet"}
        };
    }
}

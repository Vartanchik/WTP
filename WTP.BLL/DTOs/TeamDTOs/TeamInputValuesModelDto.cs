using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamInputValuesModelDto
    {
        public int GameId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string SortType { get; set; }
        public string NameValue { get; set; }
        public int WinRateLeftValue { get; set; }
        public int WinRateRightValue { get; set; }
        public int MembersLeftValue { get; set; }
        public int MembersRightValue { get; set; }
        public GoalDto[] GoalDtos { get; set; }
    }
}

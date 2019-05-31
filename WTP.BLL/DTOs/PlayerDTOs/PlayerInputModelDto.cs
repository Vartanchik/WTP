using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerInputValuesModelDto
    {
        public int GameId { get; set; } 
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string SortType { get; set; }
        public string NameValue { get; set; }
        public int RankLeftValue { get; set; }
        public int RankRightValue { get; set; }
        public int DecencyLeftValue { get; set; }
        public int DecencyRightValue { get; set; }

    }
}

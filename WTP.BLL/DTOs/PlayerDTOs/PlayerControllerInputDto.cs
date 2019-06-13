using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerControllerInputDto
    {
        public int IdGame { get; set; }
        public int PageSize { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string SortField { get; set; } = "";
        public string SortType { get; set; } = "";
        public string NameValue { get; set; } = "";
        public int RankLeftValue { get; set; }
        public int RankRightValue { get; set; }
        public int DecencyLeftValue { get; set; }
        public int DecencyRightValue { get; set; } 
    }
}

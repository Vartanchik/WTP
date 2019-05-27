using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class RankDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public GameDto Game { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string Photo { get; set; }
    }
}

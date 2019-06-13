using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerListItemOnTeamPageDto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public int Decency { get; set; }
    }
}

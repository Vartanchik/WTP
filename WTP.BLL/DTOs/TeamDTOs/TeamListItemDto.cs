using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int Coach { get; set; }
        public string Game { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        [Range(0, 100)]
        public int WinRate { get; set; }
    }
}

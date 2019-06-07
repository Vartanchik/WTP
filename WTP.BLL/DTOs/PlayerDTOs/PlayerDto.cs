using System.Collections.Generic;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Goal { get; set; }
        public int Decency { get; set; }
        public string Server { get; set; }
        public string Country { get; set; }
        public IList<string> Languages { get; set; }
        public string About { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamPhoto { get; set; }
    }
}

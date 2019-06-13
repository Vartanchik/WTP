using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerListItemDto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public string Rank { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        public string About { get; set; }
        public int Decency { get; set; }
        public IList<InvitationListItemDto> Invitations { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.ModelsDto.History;

namespace WTP.BLL.ModelsDto.Admin
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SecurityStamp { get; set; }
        public List<HistoryDto> Histories { get; set; }
    }
}

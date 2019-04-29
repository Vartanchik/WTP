using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.ModelsDto.Admin;
using WTP.BLL.ModelsDto.AdminOperation;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.BLL.ModelsDto.History
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Description { get; set; }

        public AdminDto Admin { get; set; }
        public AppUserDto AppUser { get; set; }
        public AdminOperationDto Operation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.Operation;

namespace WTP.BLL.ModelsDto.History
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Description { get; set; }

        public string PreviousUserEmail { get; set; }
        public string PreviousUserName { get; set; }
        public string NewUserEmail { get; set; }
        public string NewUserName { get; set; }

        public int AppUserId { get; set; }
        public AppUserDto AppUser { get; set; }

        public int AdminId { get; set; }

        public int? OperationId { get; set; }
        public OperationDto Operation { get; set; }
    }
}

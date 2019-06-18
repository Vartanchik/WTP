using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.AppUserDTOs
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

        public int? AdminId { get; set; }

        public int? OperationId { get; set; }
        public OperationDto Operation { get; set; }
    }
}

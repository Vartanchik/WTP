using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.ModelsDto.AppUser
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

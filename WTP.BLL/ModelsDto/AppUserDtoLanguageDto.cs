using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.TransferModels;

namespace WTP.BLL.ModelsDto
{
    public class AppUserDtoLanguageDto
    {
        public string AppUserId { get; set; }
        public AppUserDto AppUser { get; set; }
        public int LanguageId { get; set; }
        public LanguageDto Language { get; set; }
    }
}

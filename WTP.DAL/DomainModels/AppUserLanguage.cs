using System;
using System.Collections.Generic;
using System.Text;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.DAL.DomainModels
{
    public class AppUserLanguage
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}

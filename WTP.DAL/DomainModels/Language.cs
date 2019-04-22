using System.Collections.Generic;
using WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.DomainModels
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUserLanguage> AppUserLanguages { get; set; }
    }
}

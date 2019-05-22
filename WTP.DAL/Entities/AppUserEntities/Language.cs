using System.Collections.Generic;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class Language : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUserLanguage> AppUserLanguages { get; set; }
    }
}

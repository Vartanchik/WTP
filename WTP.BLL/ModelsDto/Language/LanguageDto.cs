using System.Collections.Generic;
using WTP.BLL.ModelsDto.AppUserLanguage;

namespace WTP.BLL.ModelsDto.Language
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUserDtoLanguageDto> AppUserLanguages { get; set; }
    }
}

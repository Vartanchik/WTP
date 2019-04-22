using System.Collections.Generic;
using WTP.BLL.TransferModels;

namespace WTP.BLL.ModelsDto
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUserDtoLanguageDto> AppUserLanguages { get; set; }
    }
}

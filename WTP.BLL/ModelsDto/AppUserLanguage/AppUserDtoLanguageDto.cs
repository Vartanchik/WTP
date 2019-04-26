using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.Language;

namespace WTP.BLL.ModelsDto.AppUserLanguage
{
    public class AppUserDtoLanguageDto
    {
        public int? AppUserId { get; set; }
        public AppUserDto AppUser { get; set; }
        public int LanguageId { get; set; }
        public LanguageDto Language { get; set; }
    }
}

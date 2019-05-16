namespace WTP.DAL.Entities
{
    public class AppUserLanguage
    {
        public int? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}

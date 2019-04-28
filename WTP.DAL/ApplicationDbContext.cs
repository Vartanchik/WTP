using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WTP.DAL.DomainModels;

namespace WTP.DAL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdminOperation> AdminOperations { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<int>>().HasData(
                    new { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                    new { Id = 2, Name = "User", NormalizedName = "USER" },
                    new { Id = 3, Name = "Moderator", NormalizedName = "MODERATOR" }
            );

            builder.Entity<AdminOperation>().HasData(
                    new { Id = 1, OperationName = OperationEnum.Create },
                    new { Id = 2, OperationName = OperationEnum.Update },
                    new { Id = 3, OperationName = OperationEnum.Delete },
                    new { Id = 4, OperationName = OperationEnum.Lock },
                    new { Id = 5, OperationName = OperationEnum.UnLock }
            );

            builder.Entity<AppUserLanguage>()
                .HasKey(_ => new { _.AppUserId, _.LanguageId });
            builder.Entity<AppUserLanguage>()
                .HasOne(_ => _.AppUser)
                .WithMany(_ => _.AppUserLanguages)
                .HasForeignKey(_ => _.AppUserId);
            builder.Entity<AppUserLanguage>()
                .HasOne(_ => _.Language)
                .WithMany(_ => _.AppUserLanguages)
                .HasForeignKey(_ => _.LanguageId);
        }
    }
}

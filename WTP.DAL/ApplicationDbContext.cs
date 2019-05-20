using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WTP.DAL.Entities;

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
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<int>>().HasData(
                    new { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                    new { Id = 2, Name = "User", NormalizedName = "USER" },
                    new { Id = 3, Name = "Moderator", NormalizedName = "MODERATOR" }
                );

            builder.Entity<Gender>().HasData(
                    new Gender { Id = 1, Name = "Male" },
                    new Gender { Id = 2, Name = "Female"}
                );

            builder.Entity<Country>().HasData(
                    new Country { Id = 1, Name = "Ukraine" },
                    new Country { Id = 2, Name = "Spanish" },
                    new Country { Id = 3, Name = "USA" },
                    new Country { Id = 4, Name = "Brazil" },
                    new Country { Id = 5, Name = "German" }
                );

            builder.Entity<Language>().HasData(
                    new Language { Id = 1, Name = "English" },
                    new Language { Id = 2, Name = "German" },
                    new Language { Id = 3, Name = "Russian" },
                    new Language { Id = 4, Name = "Spanish" },
                    new Language { Id = 5, Name = "Ukrainian" },
                    new Language { Id = 6, Name = "Japanese" },
                    new Language { Id = 7, Name = "Korean" },
                    new Language { Id = 8, Name = "French" },
                    new Language { Id = 9, Name = "Italian" },
                    new Language { Id = 10, Name = "Czech" },
                    new Language { Id = 11, Name = "Swedish" },
                    new Language { Id = 12, Name = "Greek" }
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

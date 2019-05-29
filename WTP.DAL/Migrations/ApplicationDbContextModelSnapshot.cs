﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WTP.DAL;

namespace WTP.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Moderator",
                            NormalizedName = "MODERATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CountryId");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("GenderId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Photo");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Steam");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("GenderId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.AppUserLanguage", b =>
                {
                    b.Property<int?>("AppUserId");

                    b.Property<int>("LanguageId");

                    b.HasKey("AppUserId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("AppUserLanguage");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Country");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Ukraine"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Spanish"
                        },
                        new
                        {
                            Id = 3,
                            Name = "USA"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Brazil"
                        },
                        new
                        {
                            Id = 5,
                            Name = "German"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Gender");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Male"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Female"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdminId");

                    b.Property<int?>("AppUserId");

                    b.Property<DateTime>("DateOfOperation");

                    b.Property<string>("Description");

                    b.Property<string>("NewUserEmail");

                    b.Property<string>("NewUserName");

                    b.Property<int>("OperationId");

                    b.Property<string>("PreviousUserEmail");

                    b.Property<string>("PreviousUserName");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("OperationId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Language");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "English"
                        },
                        new
                        {
                            Id = 2,
                            Name = "German"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Russian"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Spanish"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Ukrainian"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Japanese"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Korean"
                        },
                        new
                        {
                            Id = 8,
                            Name = "French"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Italian"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Czech"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Swedish"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Greek"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OperationName");

                    b.HasKey("Id");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ExpiryTime");

                    b.Property<int>("UserId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.RestoreToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppUserId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("RestoreTokens");
                });

            modelBuilder.Entity("WTP.DAL.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Games");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Dota 2"
                        },
                        new
                        {
                            Id = 2,
                            Name = "CS:GO"
                        },
                        new
                        {
                            Id = 3,
                            Name = "GTA V"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Goals");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Fun"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Profi"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("About");

                    b.Property<int>("AppUserId");

                    b.Property<int?>("Decency");

                    b.Property<int>("GameId");

                    b.Property<int>("GoalId");

                    b.Property<string>("Name");

                    b.Property<int>("RankId");

                    b.Property<int>("ServerId");

                    b.Property<int?>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("GameId");

                    b.HasIndex("GoalId");

                    b.HasIndex("RankId");

                    b.HasIndex("ServerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("WTP.DAL.Entities.Rank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Ranks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Uncalibrated"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Guardian"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Crusader"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Archon"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Legend"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Ancient"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Divine"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Immortal"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Servers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "East"
                        },
                        new
                        {
                            Id = 2,
                            Name = "West"
                        },
                        new
                        {
                            Id = 3,
                            Name = "North"
                        },
                        new
                        {
                            Id = 4,
                            Name = "South"
                        });
                });

            modelBuilder.Entity("WTP.DAL.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.AppUser", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("WTP.DAL.Entities.AppUserEntities.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId");
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.AppUserLanguage", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser", "AppUser")
                        .WithMany("AppUserLanguages")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.AppUserEntities.Language", "Language")
                        .WithMany("AppUserLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.History", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser", "AppUser")
                        .WithMany("Histories")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.AppUserEntities.Operation", "Operation")
                        .WithMany("Histories")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.RefreshToken", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser", "AppUser")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.Entities.AppUserEntities.RestoreToken", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.Entities.Player", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser", "AppUser")
                        .WithMany("Players")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.Goal", "Goal")
                        .WithMany()
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.Rank", "Rank")
                        .WithMany()
                        .HasForeignKey("RankId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("WTP.DAL.Entities.Team", b =>
                {
                    b.HasOne("WTP.DAL.Entities.AppUserEntities.AppUser")
                        .WithMany("Teams")
                        .HasForeignKey("AppUserId");
                });
#pragma warning restore 612, 618
        }
    }
}

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

            modelBuilder.Entity("WTP.DAL.DomainModels.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.AdminOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OperationName");

                    b.HasKey("Id");

                    b.ToTable("AdminOperations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OperationName = 1
                        },
                        new
                        {
                            Id = 2,
                            OperationName = 2
                        },
                        new
                        {
                            Id = 3,
                            OperationName = 3
                        },
                        new
                        {
                            Id = 4,
                            OperationName = 4
                        },
                        new
                        {
                            Id = 5,
                            OperationName = 5
                        });
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CountryId");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<bool>("DeletedStatus");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("GenderId");

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

            modelBuilder.Entity("WTP.DAL.DomainModels.AppUserLanguage", b =>
                {
                    b.Property<int?>("AppUserId");

                    b.Property<int>("LanguageId");

                    b.HasKey("AppUserId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("AppUserLanguage");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId");

                    b.Property<int?>("AppUserId");

                    b.Property<DateTime>("DateOfOperation");

                    b.Property<string>("Description");

                    b.Property<int?>("OperationId");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("OperationId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Teams");
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
                    b.HasOne("WTP.DAL.DomainModels.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.AppUser")
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

                    b.HasOne("WTP.DAL.DomainModels.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.AppUser", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("WTP.DAL.DomainModels.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.AppUserLanguage", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.AppUser", "AppUser")
                        .WithMany("AppUserLanguages")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WTP.DAL.DomainModels.Language", "Language")
                        .WithMany("AppUserLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.History", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.Admin", "Admin")
                        .WithMany("Histories")
                        .HasForeignKey("AdminId");

                    b.HasOne("WTP.DAL.DomainModels.AppUser", "AppUser")
                        .WithMany("Histories")
                        .HasForeignKey("AppUserId");

                    b.HasOne("WTP.DAL.DomainModels.AdminOperation", "Operation")
                        .WithMany("Histories")
                        .HasForeignKey("OperationId");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Player", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.AppUser")
                        .WithMany("Players")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("WTP.DAL.DomainModels.Team", b =>
                {
                    b.HasOne("WTP.DAL.DomainModels.AppUser")
                        .WithMany("Teams")
                        .HasForeignKey("AppUserId");
                });
#pragma warning restore 612, 618
        }
    }
}

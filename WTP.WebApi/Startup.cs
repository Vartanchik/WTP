using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WTP.BLL.ModelsDto;
using WTP.BLL.Services;
using WTP.BLL.Services.AppUserDtoService;
using WTP.BLL.Services.CountryDtoService;
using WTP.BLL.Services.GenderDtoService;
using WTP.BLL.Services.LanguageDtoService;
using WTP.BLL.Services.PlayerDtoService;
using WTP.BLL.Services.TeamDtoService;
using WTP.DAL.Services;
using WTP.WebApi.Helpers;
using WTP.WebApi.WTP.DAL;
using WTP.WebApi.WTP.DAL.DomainModels;
using WTP.WebApi.WTP.DAL.Services.AppUserService;
using WTP.WebApi.WTP.DAL.Services.AppUserServicea;
using WTP.WebApi.WTP.DAL.Services.CountryService;
using WTP.WebApi.WTP.DAL.Services.GenderService;
using WTP.WebApi.WTP.DAL.Services.LanguageService;
using WTP.WebApi.WTP.DAL.Services.PlayerService;
using WTP.WebApi.WTP.DAL.Services.TeamService;

namespace GamePlatform_WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Verbose()
             .WriteTo.ColoredConsole()
             .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();

            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IMaintainable<Country>, CountryService>();
            services.AddScoped<IMaintainable<Gender>, GenderService>();
            services.AddScoped<IMaintainable<Language>, LanguageService>();
            services.AddScoped<IMaintainable<Player>, PlayerService>();
            services.AddScoped<IMaintainable<Team>, TeamService>();

            services.AddScoped<IAppUserDtoService, AppUserDtoService>();
            services.AddScoped<IMaintainableDto<CountryDto>, CountryDtoService>();
            services.AddScoped<IMaintainableDto<GenderDto>, GenderDtoService>();
            services.AddScoped<IMaintainableDto<LanguageDto>, LanguageDtoService>();
            services.AddScoped<IMaintainableDto<PlayerDto>, PlayerDtoService>();
            services.AddScoped<IMaintainableDto<TeamDto>, TeamDtoService>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(3));

            //// In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});


            // Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

            // Conect to Database
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    _ => _.MigrationsAssembly("WTP.WebAPI")));

            // Specifiying we are going to use Identity Framework
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            // Authentication Middleware
            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = appSettings.Site,
                    ValidAudience = appSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Middleware for Authorization with requirements for our application
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireLoggedIn", policy => policy.RequireRole("Admin", "User", "Moderator")
                    .RequireAuthenticatedUser());

                options.AddPolicy("RequireAdministratorRole", policy =>
                    policy.RequireRole("Admin").RequireAuthenticatedUser());
            });
        }

        private IServiceCollection ServiceAppUser()
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
        }
    }
}

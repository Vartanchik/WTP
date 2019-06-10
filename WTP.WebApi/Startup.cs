using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using WTP.Logging;
using WTP.BLL.Services.Concrete;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.BLL.Services.Concrete.RefreshTokenService;
using WTP.BLL.Services.EmailService;
using WTP.DAL;
using WTP.DAL.UnitOfWork;
using WTP.WebAPI.Helpers;
using WTP.BLL.Services.AzureBlobStorageService;
using WTP.BLL.Services.DeleteService;
using WTP.BLL.Services.Concrete.GameService;
using WTP.BLL.Services.Concrete.PlayerSrvice;
using WTP.DAL.Entities.AppUserEntities;
using WTP.BLL.Services.Concrete.TeamService;
using WTP.BLL.DTOs.ServicesDTOs;
using System.Threading.Tasks;
using WTP.BLL.Services.Concrete.InvitationService;

namespace WTP.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<TokenSettings>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IDeleteService, DeleteService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IInvitationService, InvitationService>();

            services.AddScoped(provider => new MapperConfiguration(config =>
            {
                config.AddProfile(new DtoMapperProfile(Configuration["Photo:DefaultPhoto"]));
            }).CreateMapper());

            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<ILog, SerilogLog>();
            services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromMinutes(30));
            
            // Add the REDIS
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"]; 
            });


            // Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.WithOrigins($"{Configuration["Url:BaseUrl"]}")
                    .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

            // Conect to Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Specifiying we are going to use Identity Framework
            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
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

            }).AddRoles<IdentityRole<int>>()
              .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // Authentication Middleware
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "bearer";
                options.DefaultChallengeScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "WTP API",
                    Description = ".NET Core API",
                });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
            });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}

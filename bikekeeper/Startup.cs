using bikekeeper.Repository;
using bikekeeper.Repository.Abstract;
using bikekeeper.Services;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Models;
using bikekeeper.AutoMapper;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.HttpOverrides;

namespace bikekeeper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        // JwtConfigService
        private void JwtConfigService(IServiceCollection services)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:SecretKey"]));
            var issurer = Configuration["Jwt:JwtIssuerOptions:Issuer"];
            var audience = Configuration["Jwt:JwtIssuerOptions:Audience"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                //The signing key must match !
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                //Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issurer,

                //validate the JWT Audience (aud) claim

                ValidateAudience = true,
                ValidAudience = audience,

                //validate the token expiry
                ValidateLifetime = true,

                //
                RequireExpirationTime = true,

                // If you  want to allow a certain amout of clock drift
                ClockSkew = TimeSpan.Zero

            };

            // Make authentication compulsory across the board (i.e. shut
            // down EVERYTHING unless explicitly opened up).
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.RespectBrowserAcceptHeader = true;
                config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });            
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/Account/Forbidden/";
                    //options.LoginPath = "/Account/Unauthorized/";
                });
            services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });            
            // Configure JwtIssuerOptions
            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = issurer;
                options.Audience = audience;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                //options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                                  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<BaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));            

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Bike_Keeper",
                    Description = "API",
                    Version = "1.0"
                });
            });

            // Enabled Cors
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            services.AddControllers();

            services.AddMvc();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // Jwt
            JwtConfigService(services);

            //Mapper
            MappingConfiguration.ConfigurationService(services);

            // Register Services
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGeneralService<,>), typeof(GeneralService<,>));
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IAccountService, AccountService>();

            // Repository
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            // Firebase Account Google
            var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "keys", "bike-keeper-3374e-firebase-adminsdk-hbkgs-e3c8307dcc.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/api/swagger.json", "Swagger");
            });

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            // CORS
            app.UseCors("CorsPolicy");
            //Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

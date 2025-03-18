using DataAccessLibrary;
using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesAuto.Api.Repositories;
using SalesAuto.Api.Services;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Api
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
            services.AddDbContext<SalesAutoDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                            ,b=>b.MigrationsAssembly("SalesAuto.Api")
                            );
                        
                    }
                );

            services.AddHttpClient("CRMClientRep", config =>
            {
                config.BaseAddress = new Uri(Configuration.GetSection("CRMAPI")["APIPath"] );                
                config.Timeout = new TimeSpan(0, 0, 30);
                config.DefaultRequestHeaders.Clear();                
            });

            //services.AddScoped<ICRMClientRep, CRMClientRep>();
            services.AddTransient<ICRMClientRepo, CRMClientRepo>();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddIdentity<User,Role>().AddEntityFrameworkStores<SalesAutoDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("local",options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };                    
                })
                .AddMicrosoftIdentityWebApi( Configuration.GetSection("AzureAd"), "AzureAd")
                ;

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "AzureAd");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["JwtIssuer"],
            //        ValidAudience = Configuration["JwtAudience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
            //    };
            //});

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            services.AddControllers();            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesAuto.Api", Version = "v1" });
            });
            services.AddCors();

            services.AddTransient<ILeadsRepo, LeadsRepo>();
            services.AddTransient<IBenhNhansRepo, BenhNhansRepo>();
            services.AddTransient<IBenhViensRepo, BenhViensRepo>();
            services.AddTransient<ISaleAutoReportRepo, SaleAutoReportRepo>();
            services.AddTransient<IChiTieuSoLuongRepo, ChiTieuSoLuongRepo>();
            services.AddTransient<IRemovedLeadRepo, RemovedLeadRepo>();
            services.AddTransient<IKPIMonthlyRepo, KPIMonthlyRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<IMySqlDataAccess, MySqlDataAccess>();
            services.AddTransient<IMailRepo, MailRepo>();
            services.AddTransient<IDailyReportRepo, DailyReportRepo>();
            services.AddTransient<IABRRepo, ABRRepo>();
            services.AddTransient<IBonusTongDaiRepo, BonusTongDaiRepo>();
            services.AddTransient <IHenKhamRepo, HenKhamRepo>();
            services.AddTransient<IHis_DoiTuongRepo, His_DoiTuongRepo>();
            


            //services.AddHostedService<GetCRMDataService>();
            services.AddHostedService<SendMailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesAuto.Api v1"));
            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

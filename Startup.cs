using System.Net;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Buffers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TicketingApi.DBContexts;
using Newtonsoft.Json;
using TicketingApi.Utils;
using TicketingApi.Entities;
using TicketingApi.Middleware;
using TicketingApi.Hubs;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;

namespace TicketingApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment Env)
        {
            Configuration = configuration;
            Environment = Env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var path = Environment.ContentRootPath;
             path = path + "\\ticketing-ntf-adminsdk.json";
           // Encoding.RegisterProvider(CodePagesProvider.Instance);
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            services.AddSignalR();
            services.AddHttpClient();
            FirebaseApp.Create(new AppOptions()
            {
                // Credential = GoogleCredential.GetApplicationDefault()
                Credential = GoogleCredential.FromFile(path)
            });
            services.AddControllers() .AddNewtonsoftJson(o => 
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");  
            services.Configure<MailSetting>(Configuration.GetSection("MailSettings"));

            services.AddDbContextPool<AppDBContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))); 

            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                    builder.WithOrigins("http://localhost:3000");
                    builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("Token-Expired");;
            }));

            services.AddScoped<IFileUtil, FileUtil>();
            services.AddTransient<IMailUtil, MailUtil>();
            services.AddTransient<IFcmRequestUtil, FcmRequestUtil>();
            services.AddTransient<IHubUtil, HubUtil>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticketing Api", Version = "v1" });
            });
            if (Environment.IsProduction())
            {
                services.AddSpaStaticFiles(configuration =>
                configuration.RootPath ="ClientApp");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketing v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors("ApiCorsPolicy");

            // app.Use(async (context, next) =>
            // {
            //     var token = context.Items.ToString();
            //     if (!string.IsNullOrEmpty(token))
            //     {
            //         context.Request.Headers.Add("Authorization", "Bearer " + token);
            //     }
            //     await next();
            // });

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<HubConnection>("/hubs");
                endpoints.MapControllers();
            });
            if (env.IsProduction())
            {
                app.UseStaticFiles();
                app.UseSpaStaticFiles();
                app.UseSpa(spa =>
                {
                    spa.Options.DefaultPage = "/index.html";
                });
            }
        }
    }
}

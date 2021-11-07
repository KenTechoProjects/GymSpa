using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using APICore.Application.GymSpa.Services;
using AutoMapper;
using Domain.Application.EventTickets.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notification.Utilities.Binder;
using Persistence;
using PlayNetworkAPI.Extensions;
using Utilities;
using Utilities.Binders;

namespace PlayNetworkAPI
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
            services.AddDbContext<PNAContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DB_A57DC4_PNADb"), b => b.MigrationsAssembly("Persistence")));
            services.AddControllersWithViews();
            services.AddMvc()
            .AddNewtonsoftJson();
            services.AddControllers()
            .AddNewtonsoftJson();
            services.AddControllersWithViews();
            //services.AddSwaggerGen();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Play Network Africa [PNA] Middleware Services",
                    Description = "This API provides all the endpoints needed to extablish the Play Network Africa Services [PNA]",
                    // TermsOfService = new Uri(""),
                    Contact = new OpenApiContact
                    {
                        Name = "First Bank Sierra Leone",
                        Email = string.Empty,
                        //  Url = new Uri(" "),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Play Network Africa",
                        //  Url = new Uri(""),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.PNAIocContainer();
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            var controllerAssembly2 = Assembly.Load(new AssemblyName("APIWebInterface"));
            services.AddMvc().AddApplicationPart(controllerAssembly2).AddControllersAsServices();
            //Register Log service
            services.AddScoped<Logger>();
            //Register Appsetting (
            services.Configure<BaseUrls>(Configuration.GetSection("BaseUrls"));
            services.Configure<AppKeys>(Configuration.GetSection("AppKeys"));
            services.Configure<AppSystemType>(Configuration.GetSection("AppSystemType"));
            services.Configure<AppChannel>(Configuration.GetSection("AppChannel"));
            services.Configure<AppChannel>(Configuration.GetSection("AppChannel"));
            services.Configure<TicketSettings>(Configuration.GetSection("TicketSettings"));
            services.AddAutoMapper(typeof(Startup));
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play Network Africa API");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

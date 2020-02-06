
using ecoServer.Server.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Server.Infrastructure;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ecoPlanerWeb
{
    public class Startup
    {

        public static readonly ILoggerFactory MyLoggerFactory
             = LoggerFactory.Create(builder =>
                {
           builder.AddFilter((category, level) =>
               level == LogLevel.Warning)
                    .AddConsole();
                  });



        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }


            services.AddDbContext<EcoContext>(options =>
                options.UseLazyLoadingProxies().UseLoggerFactory(MyLoggerFactory)
                          .UseSqlServer(Configuration.GetConnectionString("Desktop")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddTransient<ContryService>();
            services.AddTransient<PopulationService>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            InitDataBase.InitDB(app.ApplicationServices);
            var task = new Task(() => GameLoop.Init(app.ApplicationServices), TaskCreationOptions.LongRunning);
            task.Start();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Mapping of endpoints goes here:
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
           {
               spa.Options.SourcePath = "ClientApp";
               spa.UseReactDevelopmentServer(npmScript: "start");

           });
        }
    }
}

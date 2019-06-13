
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Server.Infrastructure;


namespace ecoPlanerWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<EcoContext>(options =>
                options.UseSqlServer(@"Server=DESKTOP-59G7R5N;Database=test;User Id=user3; Password=hej123Hej;"));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            InitDataBase.InitDB(app.ApplicationServices);
        }
    }
}

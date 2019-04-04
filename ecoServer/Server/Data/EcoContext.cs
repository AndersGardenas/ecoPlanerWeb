
using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using Microsoft.EntityFrameworkCore;
using Server.Server.Domain.model.ResourceSet;


namespace Server.Server.Infrastructure
{
    public class EcoContext : DbContext
    {
        public EcoContext(DbContextOptions<EcoContext> options)
     : base(options)
        {
            InitDataBase.InitDB(this);
        }

        public EcoContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artisans>();
            builder.Entity<Farmer>();

            builder.Entity<ExternatlTradingResource>();
            builder.Entity<TradingResource>();

            builder.Entity<NeighbourRegion>().HasOne(r => r.OwnRegion)
                .WithMany(r => r.Negbours);

            builder.Entity<NeighbourRegion>().HasOne(r => r.NeighbouringRegion)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore<ResourceTypes>();
            builder.Ignore<ResourceType>();

            base.OnModelCreating(builder);
        }

        public DbSet<Population> Population { get; set; }

        public DbSet<Resource> Resource { get; set; }
        public DbSet<ResourceData> ResourceData { get; set; }
        public DbSet<TradingResources> TradingResources { get; set; }

        public DbSet<Demand> Demand { get; set; }

        public DbSet<Contry> Contry { get; set; }
        public DbSet<Region> Region { get; set; }

        public DbSet<InternalMarket> InternalMarket { get; set; }
        public DbSet<ExternalMarket> ExternalMarket { get; set; }
        public DbSet<NeighbourRegion> NeighbourRegion { get; set; }
    }
}

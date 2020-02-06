
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

        }

        public EcoContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Destination>();
            builder.Entity<Resource>().HasKey(re => re.ResourceId);
            builder.Entity<Resource>().HasIndex(re => re.ResourceType);
            //builder.Entity<TradingResource>().ToTable("TradingResource");
            //builder.Entity<TradingResources>().ToTable("TradingResources");
            //builder.Entity<ExternatlTradingResource>().ToTable("ExternatlTradingResource");
            builder.Entity<ResourceData>().HasIndex(re => re.ResourceType);

            //builder.Entity<TradingResources>().HasOne(r => r.InternalMarket).WithMany(r => r.Supply).HasForeignKey(r => r.InternalMarketFK).OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<TradingResources>().HasOne(r => r.InternalMarket2).WithMany(r => r.ExternalSupply).HasForeignKey(r => r.InternalMarketFK2).OnDelete(DeleteBehavior.Cascade);
            //    .HasForeignKey(r => r.InternalMarketFK).HasMany(m => m.ExternalSupply).WithOne(r => r.InternalMarket2).HasForeignKey(r => r.InternalMarketFK2);
            //builder.Entity<InternalMarket>().



            //builder.Entity<Region>().HasMany(nr => nr.Negbours)
            //    .WithOne().OnDelete(DeleteBehavior.Restrict);






            //builder.Entity<Region>().HasOne(r => r.N)
            //    .WithMany(r => r.Negbours);

            //builder.Entity<NeighbourRegion>().HasOne(r => r.NeighbouringRegion)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore<ResourceTypes>();

            base.OnModelCreating(builder);
        }

        public DbSet<Population> Population { get; set; }
        public DbSet<Farmer> Farmer { get; set; }
        public DbSet<Artisans> Artisans { get; set; }

        public DbSet<Resource> Resource { get; set; }
        public DbSet<ResourceData> ResourceData { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<TradingResource> TradingResource { get; set; }
        public DbSet<TradingResources> TradingResources { get; set; }
        public DbSet<ExternalTradingResources> ExternalTradingResources { get; set; }
        public DbSet<ExternatlTradingResource> ExternatlTradingResource { get; set; }

        public DbSet<Contry> Contry { get; set; }
        public DbSet<Region> Region { get; set; }

        public DbSet<ExternalMarket> ExternalMarket { get; set; }
        public DbSet<Destination> Destination { get; set; }
    }
}

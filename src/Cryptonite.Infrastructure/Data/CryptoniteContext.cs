using System.Diagnostics.CodeAnalysis;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Data.DataMigrations;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class CryptoniteContext : DbContext
    {
        public CryptoniteContext(DbContextOptions<CryptoniteContext> options)
            : base(options)
        {
        }

        public DbSet<DataMigration> DataMigrations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<BuyEntry> BuyEntries { get; set; }
        public DbSet<TradeEntry> TradeEntries { get; set; }
        public DbSet<Portofolio> Portofolios { get; set; }
        public DbSet<PortofolioCryptocurrency> PortofolioCryptocurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cryptonite");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoniteContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
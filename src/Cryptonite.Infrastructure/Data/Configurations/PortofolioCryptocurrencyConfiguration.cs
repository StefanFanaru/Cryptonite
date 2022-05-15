using Cryptonite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class PortofolioCryptocurrencyConfiguration : IEntityTypeConfiguration<PortofolioCryptocurrency>
    {
        public void Configure(EntityTypeBuilder<PortofolioCryptocurrency> builder)
        {
            builder.ToTable("PortofolioCryptocurrencies");
            builder.Property(x => x.PortofolioId).IsRequired().HasMaxLength(36);
            builder.Property(x => x.Symbol).IsRequired().HasMaxLength(50);
            builder.Property(x => x.InsertedAt).IsRequired();
            builder.HasKey(x => new { x.PortofolioId, x.Symbol });
        }
    }
}
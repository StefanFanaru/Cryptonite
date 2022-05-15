using Cryptonite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class TradeEntryConfiguration : IEntityTypeConfiguration<TradeEntry>
    {
        public void Configure(EntityTypeBuilder<TradeEntry> builder)
        {
            builder.Property(x => x.Id).IsRequired().HasMaxLength(36);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(36);
            builder.Property(x => x.PaidCryptocurrency).IsRequired().HasMaxLength(50);
            builder.Property(x => x.GainedCryptocurrency).IsRequired().HasMaxLength(50);
            builder.Property(x => x.GainedAmount).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(x => x.PaidAmount).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(x => x.TradedAt).IsRequired();

            builder.HasIndex(x => x.UserId);
        }
    }
}
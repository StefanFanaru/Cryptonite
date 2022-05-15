using Cryptonite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class BuyEntryConfiguration : IEntityTypeConfiguration<BuyEntry>
    {
        public void Configure(EntityTypeBuilder<BuyEntry> builder)
        {
            builder.Property(x => x.Id).IsRequired().HasMaxLength(36);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(36);
            builder.Property(x => x.PaymentCurrency).IsRequired().HasMaxLength(3);
            builder.Property(x => x.BoughtCryptocurrency).IsRequired().HasMaxLength(50);
            builder.Property(x => x.BoughtAmount).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(x => x.PaidAmount).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(x => x.BoughtAt).IsRequired();
            builder.Property(x => x.PaidUsd).IsRequired().HasColumnType("decimal(18,8)");

            builder.HasIndex(x => x.UserId);
        }
    }
}
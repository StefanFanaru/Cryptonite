using Cryptonite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class PortofolioConfiguration : IEntityTypeConfiguration<Portofolio>
    {
        public void Configure(EntityTypeBuilder<Portofolio> builder)
        {
            builder.Property(x => x.Id).IsRequired().HasMaxLength(36);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(36);
            builder.Property(x => x.Transactions).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.LastTransactionAt).IsRequired();

            builder.HasIndex(x => x.UserId);
        }
    }
}
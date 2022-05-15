using Cryptonite.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(36);
            builder.Property(x => x.PreferredCurrency).IsRequired().HasDefaultValue("USD").HasMaxLength(3);
            builder.Property(x => x.BankConversionMargin).IsRequired().HasDefaultValue(0.0m);

            builder.HasKey(x => x.UserId);
        }
    }
}
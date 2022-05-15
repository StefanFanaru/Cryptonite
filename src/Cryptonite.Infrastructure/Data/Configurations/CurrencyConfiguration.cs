using System.Collections.Generic;
using Cryptonite.Core.Common;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Cryptonite.Infrastructure.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(x => x.Date).IsRequired().HasColumnType("date");
            builder.Property(x => x.Currencies)
                .IsRequired()
                .HasColumnType("varbinary(5000)")
                .HasConversion(
                    x => StringExtensions.Zip(x.ToJson(new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.None
                    })),
                    x => StringExtensions.Unzip(x).FromJson<Dictionary<string, decimal>>());
            builder.HasKey(x => x.Date);
        }
    }
}
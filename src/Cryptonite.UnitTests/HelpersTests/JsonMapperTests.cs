using System;
using System.IO;
using Cryptonite.Infrastructure.Helpers;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Cryptonite.UnitTests.HelpersTests
{
    public class JsonMapperTests
    {
        [Fact]
        public void UtcDateTimeConverter_converts_dates()
        {
            var utcDateTimeConverter = new JsonMapper.UtcDateTimeConverter();
            var actual = utcDateTimeConverter.CanConvert(typeof(DateTime));
            actual.Should().BeTrue();
        }

        [Fact]
        public void UtcDateTimeConverter_writes_timezone_utc_date()
        {
            var utcDateTimeConverter = new JsonMapper.UtcDateTimeConverter();
            var writer = new StringWriter();
            var jsonWriter = new JsonTextWriter(writer);
            var date = new DateTime(2000, 1, 1, 1, 1, 1);
            utcDateTimeConverter.WriteJson(jsonWriter, date, new JsonSerializer());

            writer.ToString().Should().Be("\"2000-01-01T01:01:01Z\"");
        }

        [Fact]
        public void UnixDateTimeConverter_converts_dates()
        {
            var unixDateTimeConverter = new JsonMapper.UnixDateTimeConverter();
            var actual = unixDateTimeConverter.CanConvert(typeof(DateTime));
            actual.Should().BeTrue();
        }

        [Fact]
        public void UnixDateTimeConverter_writes_timezone_utc_date()
        {
            var unixDateTimeConverter = new JsonMapper.UnixDateTimeConverter();
            var jsonReader = new JsonTextReader(new StringReader("\"1632602683682\""));

            while (jsonReader.TokenType == JsonToken.None)
                if (!jsonReader.Read())
                    break;

            var actual = (DateTime)unixDateTimeConverter.ReadJson(jsonReader, typeof(long),
                null, new JsonSerializer());


            actual.Should().Be(new DateTime(2021, 09, 25, 20, 44, 43, 682));
        }
    }
}
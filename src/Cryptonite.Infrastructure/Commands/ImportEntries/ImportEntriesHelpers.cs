using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace Cryptonite.Infrastructure.Commands.ImportEntries;

public static class ImportEntriesHelpers
{
    public static List<TradeEntryImportDto> ReadTradeEntries(Stream fileStream)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(fileStream);

        var result = reader.AsDataSet();
        var table = result.Tables[0];
        var rows = table.Rows;

        var hoursDifference = ExtractHoursDifference(rows[0]);

        var entries = new List<TradeEntryImportDto>();
        for (var i = 1; i < rows.Count; i++)
        {
            var entry = new TradeEntryImportDto
            {
                TradedAt = DateTime.Parse(rows[i]["Column0"].ToString()).AddHours(hoursDifference),
                PaidAmount = decimal.Parse(rows[i]["Column5"].ToString()),
                GainedAmount = decimal.Parse(rows[i]["Column4"].ToString()),
                GainedCryptocurrencySymbol = rows[i]["Column7"].ToString(),
                PaidCryptocurrencySymbol = rows[i]["Column1"].ToString().Substring(rows[i]["Column7"].ToString().Length),
                GainedCryptocurrencyRate = decimal.Parse(rows[i]["Column3"].ToString()),
                Fee = decimal.Parse(rows[i]["Column6"].ToString())
            };
            entries.Add(entry);
        }

        return entries;
    }

    public static List<BuyEntryImportDto> ReadyBuyEntries(Stream fileStream)
    {
        using var reader = ExcelReaderFactory.CreateReader(fileStream);

        var result = reader.AsDataSet();
        var table = result.Tables[0];
        var rows = table.Rows;


        var hoursDifference = ExtractHoursDifference(rows[0]);

        var entries = new List<BuyEntryImportDto>();
        for (var i = 1; i < rows.Count; i++)
        {
            var entry = new BuyEntryImportDto
            {
                BoughtAt = DateTime.Parse(rows[i]["Column0"].ToString()).AddHours(-hoursDifference),
                PaidAmount = decimal.Parse(rows[i]["Column2"].ToString().Split(' ')[0]),
                PaymentCurrency = rows[i]["Column2"].ToString().Split(' ')[1],
                BoughtCryptoRate = decimal.Parse(rows[i]["Column3"].ToString().Split(' ')[0]),
                Fees = decimal.Parse(rows[i]["Column4"].ToString().Split(' ')[0]),
                BoughtAmount = decimal.Parse(rows[i]["Column5"].ToString().Split(' ')[0]),
                BoughtCryptocurrency = rows[i]["Column5"].ToString().Split(' ')[1]
            };

            entries.Add(entry);
        }

        return entries;
    }

    private static int ExtractHoursDifference(DataRow header)
    {
        var dateText = header["Column0"].ToString();

        if (dateText == null)
        {
            throw new Exception("Date header cell is null");
        }

        if (dateText.Contains("(UTC)"))
        {
            return 0;
        }

        var plusIndex = dateText.IndexOf("+", StringComparison.InvariantCulture);
        var minusIndex = dateText.IndexOf("-", StringComparison.InvariantCulture);

        var signIndex = plusIndex > 0 ? plusIndex : minusIndex;

        if (signIndex == -1)
        {
            throw new Exception($"Could not extract hours difference from header cell '{dateText}'");
        }

        var hoursDiff = int.Parse(dateText.Substring(signIndex + 1, dateText.Length - 2 - signIndex));
        return plusIndex > 0 ? hoursDiff : -hoursDiff;
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace TelegramBot.AzFunc.AzureCostMonitor.Csv;

public static class CsvParser
{
    public static IList<T> Parse<T>(string data)
    {
        using var csvReader = new CsvReader(new StringReader(data), CultureInfo.InvariantCulture);

        return csvReader.GetRecords<T>().ToList();
    }
}
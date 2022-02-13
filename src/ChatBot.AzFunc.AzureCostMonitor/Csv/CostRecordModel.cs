using System;
using CsvHelper.Configuration.Attributes;

namespace ChatBot.AzFunc.AzureCostMonitor.Csv;

public record CostRecordModel(
    [Index(3)]DateTimeOffset UsageDateTime,
    [Index(11)]float PreTaxCost,
    [Index(20)]string ServiceName,
    [Index(21)]string Currency
    );

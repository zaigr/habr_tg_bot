using System;
using System.Collections.Generic;
using System.Linq;
using ChatBot.AzFunc.AzureCostMonitor.Csv;

namespace ChatBot.AzFunc.AzureCostMonitor.Report;

public static class ExpenseReportBuilder
{
    public static string CreateReportMessage(IList<CostRecordModel> resourceCostRecords)
    {
        // TODO: extend with donut chart

        var computeDate = DateTimeOffset.Now.Date;
        var lastDayTotalCost = resourceCostRecords
            .Where(i => i.UsageDateTime.Date == computeDate.AddDays(-1))
            .Sum(i => i.PreTaxCost);

        return $"Cost for '{computeDate}'\n{lastDayTotalCost}$";
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using ChatBot.Api;
using ChatBot.AzFunc.AzureCostMonitor.Csv;
using ChatBot.AzFunc.AzureCostMonitor.Models;
using ChatBot.AzFunc.AzureCostMonitor.Report;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ChatBot.AzFunc.AzureCostMonitor;

public class CostExportTrigger
{
    private readonly StorageSharedKeyCredential _storageCredential;

    private readonly IBotApiClient _botApiClient;

    public CostExportTrigger(StorageSharedKeyCredential storageCredential, IBotApiClient botApiClient)
    {
        _storageCredential = storageCredential;
        _botApiClient = botApiClient;
    }

    [FunctionName("CostExportTrigger")]
    public async Task Run([EventGridTrigger]EventGridEvent @event, ILogger log)
    {
        log.LogInformation(@event.ToString());

        var eventData = JsonConvert.DeserializeObject<GridEventData>(@event.Data.ToString());

        var blobContent = await GetBlobContent(eventData);

        var reportItems = CsvParser.Parse<CostRecordModel>(blobContent);

        var message = ExpenseReportBuilder.CreateReportMessage(reportItems);
            
        log.LogInformation("Send message to chat bot.");
        await _botApiClient.SendTextMessageAsync(message);
    }

    private async Task<string> GetBlobContent(GridEventData eventData)
    {
        var client = new BlockBlobClient(new Uri(eventData.Url), _storageCredential);

        await using var blobStream = await client.OpenReadAsync();
        using var streamReader = new StreamReader(blobStream);

        return await streamReader.ReadToEndAsync();
    }
}
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services using the ConfigureServices method
//builder.Services.AddNgrok();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/gb", (Charts chart) =>
{


    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

    string filePath = "stock/GB_" + indianTime.ToString("MMM_dd_yyyy") + ".csv";// + chart.triggered_at + ".csv";

    // Create the CSV file and write the data
    return CreateCsvFile(filePath, chart, "GB");

    //return "OkResult";
});

app.MapPost("/swing", (Charts chart) =>
{

    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

    string filePath = "stock/Swing_" + indianTime.ToString("MMM_dd_yyyy") + ".csv";

    // Create the CSV file and write the data
    return CreateCsvFile(filePath, chart, "Swing");

});

app.MapGet("/sendfile", () =>
{

    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

    string swingAttachement = "stock/Swing_" + indianTime.ToString("MMM_dd_yyyy") + ".csv";
    string gbSwingAttachement = "stock/GB_" + indianTime.ToString("MMM_dd_yyyy") + ".csv";
    if(File.Exists(swingAttachement))
    GoDaddyPleskSMTPExample.EmailUtil.SendEmailWithAttachment("", "", "Summary - Swing", swingAttachement, swingAttachement);
   if(File.Exists(gbSwingAttachement))
    GoDaddyPleskSMTPExample.EmailUtil.SendEmailWithAttachment("", "", "Summary - GB", gbSwingAttachement, gbSwingAttachement);
    // Create the CSV file and write the data
    //return CreateCsvFile(filePath, chart, "Swing");

});

string CreateCsvFile(string filePath, Charts csvData, string prefix)
{
    // Create a new StreamWriter to write to the file

    var stocks = csvData.stocks;
    var stringBuilder = new StringBuilder();
    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

    if (!File.Exists(filePath))
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            foreach (string rowData in stocks.Split(','))
            {
                var line = "NSE:" + rowData + "-EQ," + indianTime.ToString();
                sw.WriteLine(line);
            }
        }
    }
    else
    {
        using (StreamWriter w = File.AppendText(filePath))
        {
            foreach (string rowData in stocks.Split(','))
            {
                var line = "NSE:" + rowData + "-EQ," + indianTime.ToString();
                w.WriteLine(line);
            }
        }
    }

    IEnumerable<string> lines = File.ReadLines(filePath);

    foreach (var li in lines)
    {
        try
        {
            var stock = li.Split(",");
            if (stock.Length > 0)
            {
                stringBuilder.Append(stock[0] + "<br />");
            }
        }
        catch { }
    }

    return GoDaddyPleskSMTPExample.EmailUtil.SendEmail(prefix + "-" + indianTime.ToString("MMM_dd_yyyy"), stringBuilder.ToString());



}

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



public class Charts
{
    public string stocks { get; set; }
    public string trigger_prices { get; set; }
    public string triggered_at { get; set; }
    public string scan_name { get; set; }
    public string scan_url { get; set; }
    public string alert_name { get; set; }
    public string webhook_url { get; set; }

}
using FinBeatTech.Database;
using FinBeatTech.Helpers;
using FinBeatTech.Middlewares;
using Microsoft.EntityFrameworkCore;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "message", new RenderedMessageColumnWriter() },
    { "template", new MessageTemplateColumnWriter() },
    { "log_level", new LevelColumnWriter() },
    { "date", new TimestampColumnWriter() },
    { "exception", new ExceptionColumnWriter() },
    { "properties", new LogEventSerializedColumnWriter() }
};
var conString = Environment.GetEnvironmentVariable("ConnectionString");
if (string.IsNullOrWhiteSpace(conString))
{
    Console.WriteLine("Connection string is not set. Please set the ConnectionString environment variable.");
    return;
}
DBMigrator.InitAndMigrate(conString);
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(le =>
                le.Properties.TryGetValue("SourceContext", out var sc) &&
                sc.ToString().Contains("LoggingMiddleware") &&
                le.Properties.TryGetValue("Scope", out var scope) &&
                scope.ToString().Contains("ApiLM"))
            .WriteTo.PostgreSQL(
                connectionString: conString,
                tableName: "logs",
                needAutoCreateTable: true,
                columnOptions: columnWriters,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
    )
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new CustomJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(conString));
var app = builder.Build();
var appUrl = Environment.GetEnvironmentVariable("APP_URL");
app.Urls.Add(appUrl??"http://0.0.0.0:8080");
app.UseMiddleware<LoggingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

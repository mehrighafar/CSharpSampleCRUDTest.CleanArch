using CSharpSampleCRUDTest.CleanArch.Core.Interfaces.DataAccess;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.DataAccessServices;
using CSharpSampleCRUDTest.CleanArch.Infrastructure.Extensions;
using CSharpSampleCRUDTest.CleanArch.UseCases;
using CSharpSampleCRUDTest.CleanArch.UseCases.PipelineBehaviors;
using CSharpSampleCRUDTest.CleanArch.Web.Configurations;
using CSharpSampleCRUDTest.CleanArch.Web.Middleware;
using dotenv.net;
using FluentValidation;
using MediatR;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

const string serviceName = "Customers";
const string serviceVersion = "1.0.0";
var instrumentation = new Instrumentation();
var activitySourceName = instrumentation.ActivitySource.Name;
var collectorEndpoint = Environment.GetEnvironmentVariable("COLLECTOR") ??
  throw new InvalidOperationException("COLLECTOR endpoint is not set.");
var lokiEndpoint = Environment.GetEnvironmentVariable("LOKI") ??
  throw new InvalidOperationException("LOKI endpoint is not set.");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddTransient<ICustomerDataAccessService, CustomerDataAccessService>();
builder.Services.AddSingleton<Instrumentation>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<OpenTelemetryContextMiddleware>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

if (builder.Environment.EnvironmentName != "Test")
{
  // Register database   
  builder.Services.AddMongoDatabase();
}
builder.Services.AddRepository();

// OpenTelemetry
builder.AddOpentTelemetryConfigs(activitySourceName, serviceName, serviceVersion, collectorEndpoint);

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Version", serviceVersion)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.AddLoggerConfigs();

logger.Information("Starting web host");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();
app.UseMiddleware<OpenTelemetryContextMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();


public partial class Program { }

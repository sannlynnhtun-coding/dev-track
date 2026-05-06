using Refit;
using DevTrack.WebApp.Services;
using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Developers;
using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Dashboard;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/webapp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7267";

// Register Refit Clients
builder.Services.AddRefitClient<IBatchApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddRefitClient<IDeveloperApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddRefitClient<ITrainingApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddRefitClient<IDashboardApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

// Feature Services (Api Versions)
builder.Services.AddScoped<IBatchService, BatchApiService>();
builder.Services.AddScoped<IDeveloperService, DeveloperApiService>();
builder.Services.AddScoped<ITrainingService, TrainingApiService>();
builder.Services.AddScoped<IDashboardService, DashboardApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

Log.CloseAndFlush();

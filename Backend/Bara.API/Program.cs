using Infrastructure.DataContext;
using Infrastructure.Repositories.FileRepositories;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.ExternalAPI_Integration;
using Services.FileStorageServices.CloudinaryStorage;
using Services.FileStorageServices.Interfaces;
using Services.YouVerifyIntegration;
using SharedModule.Settings;
using System.Text.Json.Serialization;
using UserModule.Interfaces.UserInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BaraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<Secrets>(builder.Configuration.GetSection("Secrets"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddMemoryCache();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IYouVerifyService, YouVerifyService>();
builder.Services.AddScoped<ExternalApiIntegrationService>();
builder.Services.AddScoped<IFileStorageService, CloudinaryService>();
builder.Services.AddScoped<IFileService, FileRepository>();

builder.Services.AddScoped<IWriterService, WriterRepository>();

builder.Services.AddHttpClient("YouVerify", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["AppSettings:YouVerifyBaseUrl"]}");
    client.DefaultRequestHeaders.Add("token", $"{builder.Configuration["Secrets:YouVerifyTestAPIKEY"]}");
    //client.DefaultRequestHeaders.Add("token", builder.Configuration["Secrets:YouVerifyLiveAPIKEY"]);
});

builder.Services.AddHttpClient("Cloudinary", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["AppSettings:CloudinaryBaseUrl"]}/{builder.Configuration["Secrets:CloudinaryName"]}");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

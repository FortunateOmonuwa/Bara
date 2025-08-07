using Hangfire;
using Infrastructure.DataContext;
using Infrastructure.Repositories.FileRepositories;
using Infrastructure.Repositories.ScriptRepositories;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScriptModule.Interfaces;
using Serilog;
using Services.BackgroudServices;
using Services.ExternalAPI_Integration;
using Services.FileStorageServices.CloudinaryStorage;
using Services.FileStorageServices.Interfaces;
using Services.MailingService;
using Services.MailingService.SendGrid;
using Services.SignalR;
using Services.YouVerifyIntegration;
using SharedModule.Settings;
using SharedModule.Utils;
using System.Text;
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

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("Connection"))
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings();
});
builder.Services.AddHangfireServer();
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 2,
    DelaysInSeconds = [10, 30],
    OnAttemptsExceeded = AttemptsExceededAction.Fail
});

builder.Services.AddScoped<HangfireJobs>();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IYouVerifyService, YouVerifyService>();
builder.Services.AddTransient<ExternalApiIntegrationService>();
builder.Services.AddTransient<IFileStorageService, CloudinaryService>();
builder.Services.AddTransient<IFileService, FileRepository>();
builder.Services.AddTransient<IMailService, SendGridService>();
builder.Services.AddTransient<IWriterService, WriterRepository>();
builder.Services.AddTransient<IScriptService, ScriptRepository>();
builder.Services.AddTransient<IProducerService, ProducerRepository>();
builder.Services.AddTransient<IAuthService, AuthRepository>();
builder.Services.AddScoped(typeof(LogHelper<>));

builder.Services.AddSignalR();
//var retryPolicy = HttpPolicyExtensions
//    .HandleTransientHttpError()
//    .OrResult(msg => (int)msg.StatusCode == 429) 
//    .WaitAndRetryAsync(
//        retryCount: 3,
//        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
//        onRetry: (outcome, timespan, retryAttempt, context) =>
//        {
//            Console.WriteLine($"Retrying... Attempt {retryAttempt}");
//        });
//builder.Services.AddHttpClient("default", client =>
//{
//    client.Timeout = TimeSpan.FromSeconds(30);
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//})
//.AddPolicyHandler(retryPolicy);

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{builder.Configuration["Secrets: JwtSickCrit"]}")),
        ValidIssuers = [builder.Configuration["Secrets:Issuers"]],
        RoleClaimType = "Role",
        NameClaimType = "UserId",
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["AccessToken"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notification"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()
.AddPolicy("VerifiedOnly", policy =>
{
    policy.RequireClaim("VerificationStatus", "Verified");
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Bara", new OpenApiInfo { Title = "Bara-API" });

    var basePath = AppContext.BaseDirectory;
    var xmlDocs = Directory.GetFiles(basePath, "*.xml");

    foreach (var xmlPath in xmlDocs)
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Bearer Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });

});
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<NotificationHub>("/notification");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

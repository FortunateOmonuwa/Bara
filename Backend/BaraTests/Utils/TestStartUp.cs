using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.ExternalAPI_Integration;
using Services.YouVerifyIntegration;
using SharedModule.Settings;

namespace BaraTests.Utils
{
    internal class TestStartUp
    {
        private static IServiceProvider _provider;

        static TestStartUp()
        {
            _provider = Build();
        }
        public static IServiceProvider Build()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Utils/test.config.json", false, false)
                .Build();

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<Secrets>(configuration.GetSection("Secrets"));
            services.AddTransient<ExternalApiIntegrationService>();
            services.AddTransient<IYouVerifyService, YouVerifyService>();
            services.AddHttpClient("YouVerify", client =>
            {
                client.BaseAddress = new Uri(configuration["AppSettings:YouVerifyBaseUrl"]);
                client.DefaultRequestHeaders.Add("token", configuration["Secrets:YouVerifyTestAPIKEY"]);
                //client.DefaultRequestHeaders.Add("token", configuration["Secrets:YouVerifyLiveAPIKEY"]);
            });
            services.AddLogging();

            return services.BuildServiceProvider();
        }

        public static T Resolve<T>() where T : notnull
        => _provider.GetRequiredService<T>();
    }
}

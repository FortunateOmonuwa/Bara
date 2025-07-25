using Microsoft.Extensions.Options;
using Services.YouVerifyIntegration;
using SharedModule.Settings;

namespace BaraTests.Utils
{
    public class BaseTestFixture
    {
        protected readonly IYouVerifyService youVerify;
        protected readonly AppSettings settings;
        protected readonly Secrets secrets;

        protected BaseTestFixture()
        {
            youVerify = TestStartUp.Resolve<IYouVerifyService>();
            settings = TestStartUp.Resolve<IOptions<AppSettings>>().Value;
            secrets = TestStartUp.Resolve<IOptions<Secrets>>().Value;
        }
    }
}

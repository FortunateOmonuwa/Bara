using MailKit;
using Microsoft.Extensions.Options;
using ScriptModule.Interfaces;
using Services.BackgroudServices;
using Services.FileStorageServices.Interfaces;
using Services.YouVerifyIntegration;
using SharedModule.Settings;
using UserModule.Interfaces.UserInterfaces;

namespace BaraTests.Utils
{
    public class BaseTestFixture
    {
        protected readonly IYouVerifyService youVerify;
        protected readonly AppSettings settings;
        protected readonly Secrets secrets;
        protected readonly HangfireJobs hangfire;
        protected readonly IFileService fileService;
        protected readonly IFileStorageService fileStorageService;
        protected readonly IMailService mailService;
        protected readonly IScriptService scriptService;
        protected readonly IWriterService writerService;
        protected readonly IProducerService producerService;
        protected readonly IAuthService authService;
        protected BaseTestFixture()
        {
            youVerify = TestStartUp.Resolve<IYouVerifyService>();
            settings = TestStartUp.Resolve<IOptions<AppSettings>>().Value;
            secrets = TestStartUp.Resolve<IOptions<Secrets>>().Value;
            hangfire = TestStartUp.Resolve<HangfireJobs>();
            fileService = TestStartUp.Resolve<IFileService>();
            fileStorageService = TestStartUp.Resolve<IFileStorageService>();
            mailService = TestStartUp.Resolve<IMailService>();
            scriptService = TestStartUp.Resolve<IScriptService>();
            writerService = TestStartUp.Resolve<IWriterService>();
            producerService = TestStartUp.Resolve<IProducerService>();
            authService = TestStartUp.Resolve<IAuthService>();
        }
    }
}

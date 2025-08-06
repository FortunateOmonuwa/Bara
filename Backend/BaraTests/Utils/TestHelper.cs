using Microsoft.Extensions.Configuration;

namespace BaraTests.Utils
{
    internal static class TestHelper
    {
        public static string YouVerifyBaseUrl { get; set; }
        public static string YouVerify_BVN_VerificationUrl { get; set; }
        public static string YouVerify_Drivers_License_VerificationUrl { get; set; }
        public static string YouVerify_NIN_VerificationUrl { get; set; }
        public static string YouVerify_International_Passport_VerificationUrl { get; set; }

        //File Storage
        public static string AzureBlobStoragePath { get; set; }
        public static string GoogleDriveStoragePath { get; set; }

        //Mailer
        public static string Sender { get; set; }
        public static int Port { get; set; }
        public static string Server { get; set; }
        public static string Password { get; set; }

        //Mailer (SMTP)
        public static string SMTP_Server { get; set; }
        public static string SMTP_Sender { get; set; }
        public static int SMTP_Port { get; set; }
        public static string SMTP_DisplayName { get; set; }
        public static string SMTP_Password { get; set; }

        // JWT
        public static string SecretKey { get; set; }
        public static string JwtSickRit { get; set; }
        public static List<string> Issuers { get; set; }
        public static string IpInfoKey { get; set; }

        //TEST CREDENTIALS
        static TestHelper()
        {
            var configuration = new ConfigurationBuilder()
                                     .SetBasePath(Directory.GetCurrentDirectory())
                                     .AddJsonFile("Utils/test.config.json", false, false)
                                     .Build();

            YouVerifyBaseUrl = configuration["YouVerifyBaseUrl"];
            YouVerify_BVN_VerificationUrl = configuration["YouVerify_BVN_VerificationUrl"];
            YouVerify_Drivers_License_VerificationUrl = configuration["YouVerify_Drivers_License_VerificationUrl"];
            YouVerify_NIN_VerificationUrl = configuration["YouVerify_NIN_VerificationUrl"];
            YouVerify_International_Passport_VerificationUrl = configuration["YouVerify_International_Passport_VerificationUrl"];
            AzureBlobStoragePath = configuration["AzureBlobStoragePath"];
            GoogleDriveStoragePath = configuration["GoogleDriveStoragePath"];
            Sender = configuration["Sender"];
            Port = int.Parse(configuration["Port"]);
            Server = configuration["Server"];
            Password = configuration["Password"];
            SMTP_Server = configuration["SMTP_Server"];
            SMTP_Sender = configuration["SMTP_Sender"];
            SMTP_Port = int.Parse(configuration["SMTP_Port"]);
            SMTP_DisplayName = configuration["SMTP_DisplayName"];
            SMTP_Password = configuration["SMTP_Password"];
            SecretKey = configuration["SecretKey"];
            //Issuers = configuration["Issuers"];
            JwtSickRit = configuration["JwtSickRit"];
            IpInfoKey = configuration["IpInfoKey"];
        }
    }
}

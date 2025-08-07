﻿namespace SharedModule.Settings
{
    public class Secrets
    {
        public string YouVerifyTestAPIKEY { get; set; }
        public string YouVerifyLiveAPIKEY { get; set; }
        public string YouVerifyWebhookSigningSecret { get; set; }
        public string CloudinaryAPIKEY { get; set; }
        public string CloudinaryAPISecret { get; set; }
        public string CloudinaryURL { get; set; }
        public string CloudinaryName { get; set; }
        public string CloudinaryFolderName { get; set; }
        public string JwtSickRit { get; set; }
        public List<string> Issuers { get; set; }
        public string IpInfoKey { get; set; }
        public string RabbitMqHost { get; set; }
        public int RabbitMqPort { get; set; }
        public string RabbitMqUsername { get; set; }
        public string RabbitMqPassword { get; set; }
    }
}

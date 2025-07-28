namespace Services.MailingService
{
    internal class MailNotifications
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string RecipientEmail { get; set; }
        public string SenderEmail { get; set; }
        public DateTime SentDate { get; set; }
        public MailNotifications(string subject, string body, string recipientEmail, string senderEmail)
        {
            Subject = subject;
            Body = body;
            RecipientEmail = recipientEmail;
            SenderEmail = senderEmail;
            SentDate = DateTime.Now;
        }

        private static string BaseEmailTemplate(string name = "", string bodyContent = "")
        {
            var mail = $@"
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Bara Platform</title>
                        <style>
                            body {{
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                background-color: #F4F4F4;
                                padding: 20px;
                                margin: 0;
                                color: #DADBDD;
                            }}
                            .bara{{
                                color: #DADBDD;    
                                font-weight: bold;
                                font-size:1rem;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #111215;
                                padding: 30px;
                                border-radius: 8px;
                                box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
                                border: 1px solid #333740;
                            }}
                            .header {{
                                text-align: center;
                                margin-bottom: 30px;
                                padding-bottom: 20px;
                                border-bottom: 2px solid #BF0000;
                            }}
                            .logo {{
                                font-size: 24px;
                                font-weight: bold;
                                color: #BF0000;
                                margin-bottom: 5px;
                            }}
                            .subtitle {{
                                color: #858990;
                                font-size: 14px;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 12px 24px;
                                margin: 20px 0;
                                color: #FFFFFF !important;
                                background-color: #BF0000;
                                border: none;
                                border-radius: 5px;
                                text-decoration: none;
                                font-weight: 500;
                                font-size: 16px;
                            }}
                            .button:hover {{
                                background-color: #800000;
                            }}
                            p {{
                                line-height: 1.6;
                                margin-bottom: 15px;
                                color: #DADBDD;
                            }}
                            .footer {{
                                margin-top: 30px;
                                padding-top: 20px;
                                border-top: 1px solid #333740;
                                font-size: 14px;
                                color: #858990;
                            }}
                            .contact-info {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #696D77;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""email-container"">
                            <div class=""header"">
                                <div class=""logo"">Bara</div>
                                <div class=""subtitle"">Collaborate. Create. Own.</div>
                            </div>

                            <h3>Hi{(!string.IsNullOrWhiteSpace(name) ? " " + name : "")}, </h3>
                            {bodyContent}

                            <div class=""footer"">
                                <p>Warm regards,<br><strong>The Bara Team</strong></p>
                                <div class=""contact-info"">
                                    <p>This is an automated message from Bara.<br>
                                    Need help? Reach out to our support team @ baraglobalmain@gmail.com.</p>
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>";
            return mail;
        }

        public static object RegistrationConfirmationMailNotification(string receiver, string name, string token)
        {
            var subject = "WELCOME TO BARA!";
            var body = $@"
                <br/>
                <p>Thank you for joining <b className=""bara"">Bara!!!</b> We're thrilled to have you on board.</p>
                <p>Please verify your account with this token <b classNamw=""bara"">{token.ToUpper()}</b></p>
                <p>It expires in 5 mins";
            return new
            {
                Receiver = receiver,
                Subject = subject,
                Body = BaseEmailTemplate(name, body),
            };
        }

        public static object EmailVerifiedNotification(string receiver, string name)
        {
            var subject = "EMAIL VERIFIED";
            var body = $@"
                <br/>
                <p>Congratulations {name}, your email has been successfully verified.</p>";
            return new
            {
                Receiver = receiver,
                Subject = subject,
                Body = BaseEmailTemplate(name, body),
            };
        }

        public static object AccountVerificationSuccessNotification(string receiver, string name)
        {
            var subject = "ACCOUNT VERIFICATION SUCCESSFUL";
            var body = $@"
                <br/>
                <p>Congratulations {name}, your account has been successfully verified.</p>";
            return new
            {
                Receiver = receiver,
                Subject = subject,
                Body = BaseEmailTemplate(name, body),
            };
        }
    }
}

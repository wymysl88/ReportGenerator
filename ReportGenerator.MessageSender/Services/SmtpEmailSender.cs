using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ReportGenerator.MessageSender
{
    public class SmtpEmailSender : IResponseMessageSender
    {
        private readonly SmtpClient smtpClient;

        private readonly string emailFrom;

        public SmtpEmailSender()
        {
            var smtpUrl = Environment.GetEnvironmentVariable("SmtpUrl");
            var emailAccount = Environment.GetEnvironmentVariable("EmailAccount");
            var emailPassword = Environment.GetEnvironmentVariable("EmailPassword");

            this.emailFrom = Environment.GetEnvironmentVariable("EmailFrom");

            this.smtpClient = new SmtpClient(smtpUrl)
            {
                Port = 587,
                Credentials = new NetworkCredential(emailAccount, emailPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task SendMessageAsync(string fileUrl, string recipient)
        {
            try
            {
                var message = GetEmailMessage(fileUrl, recipient);

                await this.smtpClient.SendMailAsync(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR while sending an Smtp Email\n\t{0}\n", ex.Message);
            }
        }

        private MailMessage GetEmailMessage(string fileUrl, string recipient)
        {
            var from = new MailAddress(emailFrom);
            var to = new MailAddress(recipient);
            var message = new MailMessage(from, to)
            {
                Subject = "[ReportGenerator] Your report file is ready",
                Body = $"Report is ready. It's available to download here: {fileUrl}"
            };

            return message;
        }
    }
}

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ReportGenerator.MessageSender
{
    public class SendGridEmailSender : IResponseMessageSender
    {
        private readonly string emailFrom;

        private readonly SendGridClient client;

        public SendGridEmailSender()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");

            this.emailFrom = Environment.GetEnvironmentVariable("EmailFrom");
            this.client = new SendGridClient(apiKey);
        }

        public async Task SendMessageAsync(string fileUrl, string recipient)
        {
            try
            {
                var mailMessage = GetEmailMessage(fileUrl, recipient);

                var response = await client.SendEmailAsync(mailMessage);

                if (!response.IsSuccessStatusCode)
                {
                    Console.Write("ERROR - Sending SendGrid email failed");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR while sending an SendGrid Email\n\t{0}\n", ex.Message);
            }
        }

        private SendGridMessage GetEmailMessage(string fileUrl, string recipient)
        {
            var from = new EmailAddress(emailFrom);
            var to = new EmailAddress(recipient);

            var subject = "[ReportGenerator] Your report file is ready";
            var body = $"Report is ready. It's available to download here: {fileUrl}";
         
            var message = MailHelper.CreateSingleEmail(from, to, subject, body, null);

            return message;
        }
    }
}

using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Email.Contracts;
using TemplateV2.Infrastructure.Email.Models;

namespace TemplateV2.Infrastructure.Email
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly EmailSettings _settings;
        private readonly ISendGridClient _client;

        public SendGridEmailProvider(IOptions<EmailSettings> emailSettings)
        {
            _settings = emailSettings.Value;
            _client = new SendGridClient(_settings.SendGrid.APIKey);
        }

        public async Task Send(SendRequest request)
        {
            var from = new EmailAddress(request.FromAddress);
            var to = new EmailAddress(request.ToAddress);
            var plainTextContent = Regex.Replace(request.Body, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, plainTextContent, request.Body);
            var response = await _client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                //todo: handle this better and log
                throw new System.Exception("Could not send email");
            }
        }
    }
}

namespace TemplateV2.Infrastructure.Email.Models
{
    public class SendRequest
    {
        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}

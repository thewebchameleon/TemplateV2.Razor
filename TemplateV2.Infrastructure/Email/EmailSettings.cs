namespace TemplateV2.Infrastructure.Email
{
    public class EmailSettings
    {
        public bool IsEnabled { get; set; }

        public SendGridConfig SendGrid { get; set; }

        public string SystemEmailAddress { get; set; }

        public string SystemEmailName { get; set; }

        public string RecipientEmailAddress { get; set; }
    }
}

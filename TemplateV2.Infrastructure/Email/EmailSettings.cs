namespace TemplateV2.Infrastructure.Email
{
    public class EmailSettings
    {
        public SmtpSettings Smtp { get; set; }
    }

    public class SmtpSettings
    {
        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }
    }
}

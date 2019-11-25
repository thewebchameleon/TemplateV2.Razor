namespace TemplateV2.Models.ServiceModels.Email
{
    public class SendFeedbackRequest
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Message { get; set; }
    }
}

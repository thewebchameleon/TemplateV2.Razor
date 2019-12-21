namespace TemplateV2.Models
{
    public class OpenGraphViewModel
    {
        public string Url { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public MetaTagImage Image { get; set; }

        public string FacebookAppId { get; set; }
    }

    public class MetaTagImage
    {
        public string Url { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string ContentType { get; set; }
    }
}

namespace TemplateV2.Infrastructure.Cache
{
    public class CacheSettings
    {
        public bool IsEnabled { get; set; }

        public int ExpiryTimeMinutes { get; set; }
    }
}

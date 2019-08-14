namespace TemplateV2.Common.Extensions
{
    public static class StringExtensions
    {
        public static string SafeTrim(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.Trim();
        }
    }
}

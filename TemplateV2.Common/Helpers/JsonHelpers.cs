using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TemplateV2.Common.Helpers
{
    public static class JsonHelper
    {
        public static string ObfuscateFieldValues(this string jsonData, List<string> fieldNames)
        {
            var propertiesToMask = new HashSet<string>(fieldNames);
            var jObj = JObject.Parse(jsonData);

            var fieldValuesToObfuscate = new List<string>();

            foreach (var p in jObj.Descendants()
                                 .OfType<JProperty>()
                                 .Where(p => propertiesToMask.Contains(p.Name)))
            {
                fieldValuesToObfuscate.Add(p.Value.ToString());
            }

            foreach (var fieldValue in fieldValuesToObfuscate)
            {
                if (!string.IsNullOrEmpty(fieldValue)) // in case the field value is empty
                {
                    jsonData = jsonData.Replace(fieldValue, "******");
                }
            }

            return jsonData;
        }
    }
}

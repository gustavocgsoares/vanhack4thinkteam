using Farfetch.Application.Model.Enums.Base;
using Newtonsoft.Json;

namespace Farfetch.Application.Model.Contexts.Base
{
    public class Link
    {
        public string Href { get; set; }

        [JsonProperty(PropertyName = "rel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Relations { get; set; }

        public Method Method { get; set; }
    }
}

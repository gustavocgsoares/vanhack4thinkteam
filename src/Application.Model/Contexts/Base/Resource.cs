using System.Collections.Generic;
using Newtonsoft.Json;

namespace Farfetch.Application.Model.Contexts.Base
{
    public abstract class Resource
    {
        [JsonProperty(Order = -2)]
        public List<Link> Links { get; set; }
    }
}

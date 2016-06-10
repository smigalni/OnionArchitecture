using System;
using Newtonsoft.Json;

namespace OnionArchitectureExample2
{
    public class CrawlerEntity
    {

        //[JsonProperty("data")]
        //public T Data { get; private set; }

        [JsonProperty("updated")]
        public DateTime Updated { get;  set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; private set; }

        [JsonProperty("type")]
        public string Type { get;  set; }

        [JsonProperty("operatingDayOslo")]
        public string OperatingDay { get; set; }

        //[JsonProperty("category")]
        //public string Category { get; set; }
    }
}

using System;
using OnionArchitectureExample;
using Newtonsoft.Json;

namespace OnionArchitectureExample
{

    public class StatusEntity
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; private set; }

        public DateTime LastSeen { get; set; }

        public string Type { get; set; }
        public int Status { get; set; }

        public string UserFriendlyStatus { get; set; }

        public static StatusEntity Create(string id, DateTime now, StatusEnum serviceStatus, string type)
        {
            return new StatusEntity()
            {
                Id = id,
                LastSeen = now,
                Status = 1, 
                UserFriendlyStatus = serviceStatus.ToString(),
                Type = type
            };
        }
    }
}

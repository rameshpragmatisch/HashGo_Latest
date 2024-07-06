using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class EstimateWaitingTimeResponseData
    {
        [JsonPropertyName("IsError")]
        public bool IsError { get; set; }

        [JsonPropertyName("Response")]
        public EstimateWaitingTimeResponse Response { get; set; }

        [JsonPropertyName("Error")]
        public object Error { get; set; }
    }

    public class EstimateWaitingTimeResponse
    {
        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Id")]
        public int Id { get; set; }
    }
}

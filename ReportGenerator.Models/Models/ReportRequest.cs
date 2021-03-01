using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReportGenerator.Models
{
    public class ReportRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("recipientEmail")]
        public string RecipientEmail { get; set; }

        [JsonProperty("data")]
        public List<dynamic> Data { get; set; }
    }
}

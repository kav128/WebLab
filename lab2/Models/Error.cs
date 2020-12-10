#nullable disable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record Error
    {
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_msg")]
        public string ErrorMessage { get; set; }
        
        [JsonPropertyName("request_params")]
        public ICollection<Parameter> RequestParams { get; set; }
    }
}

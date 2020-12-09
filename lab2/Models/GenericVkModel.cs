using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace lab2.Models
{
    public record GenericVkModel
    {
        [JsonPropertyName("error")]
        public Error? Error { get; set; }
    }
}

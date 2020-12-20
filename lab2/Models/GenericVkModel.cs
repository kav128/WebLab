using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record GenericVkModel
    {
        [JsonPropertyName("error")]
        public Error? Error { get; set; }
    }
}

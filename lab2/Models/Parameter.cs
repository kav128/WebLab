using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record Parameter
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
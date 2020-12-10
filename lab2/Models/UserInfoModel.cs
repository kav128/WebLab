#nullable disable
using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record UserInfoModel
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("photo_100")]
        public string ProfilePic100 { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record UserInfoModel
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
    }
    
    public record ApiResponseModel<TModel>
    {
        [JsonPropertyName("response")]
        public ICollection<TModel>? Response { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab2.Models
{
    public record ApiResponseModel<TModel>
    {
        [JsonPropertyName("response")]
        public ICollection<TModel>? Response { get; set; }

        [JsonPropertyName("error")]
        public Error? Error { get; set; }
    }
}
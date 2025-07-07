using System.Text.Json.Serialization;

namespace WebApp.Model
{
    public class JsonWebToken
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}

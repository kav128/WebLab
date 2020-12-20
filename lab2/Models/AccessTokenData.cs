namespace lab2.Models
{
    public record AccessTokenData
    {
        public string AccessToken { get; init; }

        public int ExpiresIn { get; init; }

        public int UserId { get; init; }
    }
}
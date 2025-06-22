namespace Casino.API.Infrastructure.JWT
{
    public class JWTConfig
    {
        public string? Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}

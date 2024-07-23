namespace Project.API.Utils
{
    // Jwt configurations
    public class JwtSettings
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int LifeTime { get; set; }
        public string Secret { get; set; } = null!;
        public string SigningKey { get; set; } = null!;
    }
}

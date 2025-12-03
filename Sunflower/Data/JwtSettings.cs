namespace Sunflower.Data
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = "Sunflower";
        public string Audience { get; set; } = "SunflowerUsers";
        public int ExpiryMinutes { get; set; } = 60;
    }
}
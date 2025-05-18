namespace auth_service.Models
{
    public class JWTsettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenLifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
    }
}

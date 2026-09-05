namespace FinTrack.Application.Configuration
{
    public class JwtSettings
    {
        public TimeSpan AccessTokenLifetime { get; set; }
        public TimeSpan RefreshTokenLifetime { get; set; }
        public required string Issuer { get; set; }//store who created JWT to verify if the given token issued by current project during another service/api receives the jwt
        public required string Audience { get; set; }// identify who teh token intended for here issuer=>fintrack,audience=>fintrack.api
        public required string SecretKey { get; set; }//to cehck if token hasnt been modified

    }
}
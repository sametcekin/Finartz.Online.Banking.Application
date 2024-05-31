namespace Domain.AppSettings
{
    public class TokenSettings
    {
        public string Secret { get; set; }

        public int AccessTokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }
    }
}

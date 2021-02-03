namespace NetBlade.Core.Security.Options
{
    public class TokenOption
    {
        public int ExpiresInMinutes { get; set; }

        public string FormsDecripitionKey { get; set; }

        public string FormsValidationKey { get; set; }

        public string Issuer { get; set; }

        public string PrivateKey { get; set; }

        public string SecurityAlgorithms { get; set; }
    }
}

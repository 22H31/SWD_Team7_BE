namespace BE_Team7.Sevices
{
    public static class TokenRevocationService
    {
        private static readonly HashSet<string> RevokedTokens = new();

        public static void RevokeToken(string token)
        {
            RevokedTokens.Add(token);
        }

        public static bool IsTokenRevoked(string token)
        {
            return RevokedTokens.Contains(token);
        }
    }
}
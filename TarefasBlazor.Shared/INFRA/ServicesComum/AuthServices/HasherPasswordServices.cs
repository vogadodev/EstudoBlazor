using System.Security.Cryptography;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.AuthServices
{
    public  static class HasherPasswordServices
    {
        public static void CreatedHashPassword(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new Rfc2898DeriveBytes(password, 32, 100_000, HashAlgorithmName.SHA512);
            salt = hmac.Salt;
            hash = hmac.GetBytes(64);
        }

        public static bool CheckPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA512);
            var hashComparado = hmac.GetBytes(64);
            return CryptographicOperations.FixedTimeEquals(hashComparado, hash);
        }
    }
}

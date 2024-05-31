using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class PasswordHasher
{
    const int keySize = 64;
    const int iterations = 350000;
    private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
    public static string HashPasword(string password, out string salt)
    {
        var passwordSalt = RandomNumberGenerator.GetBytes(keySize);
        salt = Convert.ToHexString(passwordSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                             passwordSalt,
                                             iterations,
                                             hashAlgorithm,
                                             keySize);
        return Convert.ToHexString(hash);
    }
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var passwordSalt = Convert.FromHexString(salt);
        var currentHash = Convert.FromHexString(hash);
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), passwordSalt, iterations, hashAlgorithm, keySize);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, currentHash);
    }
}

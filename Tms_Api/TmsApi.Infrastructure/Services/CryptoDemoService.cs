namespace TmsApi.Infrastructure.Services;

public class CryptoDemoService
{
    public string HashUserPassword(string plainText)
    {
        // BCrypt generates a random salt internally and embeds it in the
        // output string — you never store the salt as a separate column.
        // workFactor: 12 = 2^12 (4096) key-derivation rounds. Higher = slower
        // to compute = more brute-force resistant. 12 is a solid default.
        return BCrypt.Net.BCrypt.HashPassword(plainText, workFactor: 12);
    }

    public bool VerifyUserPassword(string plainText, string hashedDbPassword)
    {
        // BCrypt reads the salt back out of the stored hash before comparing,
        // which is why Verify only needs the plaintext + the hash — nothing else.
        return BCrypt.Net.BCrypt.Verify(plainText, hashedDbPassword);
    }
}
using System.Security.Cryptography;
using System.Text;

namespace Pharmacy.Infrastructure.EncryptionServices;
public class TokenGenerationService: ITokenGenerationService
{
    #region Fields

    private readonly IMemoryCache _memoryCache;
    private readonly string _encryptionKey;

    #endregion

    #region Ctor

    public TokenGenerationService(IMemoryCache memoryCache, IConfiguration _configuration)
    {
        _memoryCache = memoryCache;
        _encryptionKey = _configuration.GetValue<string>("TokenEncryptionKey");
    }

    #endregion

    #region Public Methods

    public async ValueTask<string> GenerateTokenAsync(Guid pharmacyId, Guid customerId)
    {
        var token = new TokenDto()
        {
            CustomerId = customerId,
            PharmacyId = pharmacyId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var initializationVector = GenerateRandomPublicKey();
        var key = Encoding.UTF8.GetByteCount(_encryptionKey) == 32 ? Encoding.UTF8.GetBytes(_encryptionKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_encryptionKey));


        var encryptString = await EncryptAsync(System.Text.Json.JsonSerializer.Serialize(token), key, initializationVector);

        _memoryCache.Set($"{pharmacyId},{customerId}", initializationVector, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(11)
        });

        return encryptString;
    }

    public async ValueTask<bool> VerifyTokenAsync(string encryptedString, Guid pharmacyId, Guid customerId)
    {
        var initializationVector = _memoryCache.Get<byte[]>($"{pharmacyId},{customerId}");

        if (string.IsNullOrEmpty(encryptedString) || initializationVector == null || initializationVector.Length <= 0)
        {
            throw new Exception("Bad Request. Invalid encryption string.");
        }


        var key = Encoding.UTF8.GetByteCount(_encryptionKey) == 32 ? Encoding.UTF8.GetBytes(_encryptionKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_encryptionKey));

        string decryptedString = await DecryptAsync(encryptedString, key, initializationVector);

        var token = System.Text.Json.JsonSerializer.Deserialize<TokenDto>(decryptedString);

        double diffInMinute = (DateTime.UtcNow - token.CreatedAtUtc).TotalMinutes;

        if (diffInMinute < 11)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Private Methods

    private async ValueTask<string> DecryptAsync(string encryptedString, byte[] key, byte[] initializationVector)
    {
        if (key == null || key.Length <= 0)
        {
            throw new Exception("Invalid Key");
        }

        string plainText = string.Empty;

        using (var aes = Aes.Create())
        {
            aes.Mode = CipherMode.CBC;
            aes.Key = key;
            aes.IV = initializationVector;

            var cTransform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(Convert.FromBase64String(encryptedString)))
            {
                using (var cStream = new CryptoStream(ms, cTransform, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cStream))
                    {
                        plainText = await sr.ReadToEndAsync();
                    }
                }
            }
        }
 
        return plainText;
    }

    private async ValueTask<string> EncryptAsync(string plainText, byte[] key, byte[] initializationVector)
    {
        if (key == null || key.Length <= 0)
        {
            throw new Exception("Invalid Key");
        }

        byte[] encryptedByte;

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.IV = initializationVector;

            var cTransform= aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cStream = new CryptoStream(ms, cTransform, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cStream))
                    {
                        await sw.WriteAsync(plainText);
                    }

                    encryptedByte = ms.ToArray();
                }
            }
        }

        return Convert.ToBase64String(encryptedByte);
    }

    public static byte[] GenerateRandomPublicKey()
    {
        var iv = new byte[16]; 
        iv = RandomNumberGenerator.GetBytes(iv.Length);
        return iv;
    }

    #endregion
}

public class TokenDto
{
    public Guid CustomerId { get; set; }
    public Guid PharmacyId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}


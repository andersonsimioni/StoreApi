using System.Security.Cryptography;
using System.Text;

namespace StoreApi.Shared.Services.Cryptography;

public class Cryptography : ICryptography
{
    private readonly byte[] Aes256CbcKey;
    private readonly byte[] Aes256CbcInitialVector;

    public string Decrypt(string data)
    {
        var byteBuffer = Enumerable.Range(0, data.Length) .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                             .ToArray();

        var normalText = DecodeAES256CBC(byteBuffer, this.Aes256CbcKey, this.Aes256CbcInitialVector);
        var originalData = ConvertFromRandomFormat(normalText);
        return Encoding.Default.GetString(originalData);
    }

    public string Encrypt(string data)
    {
        var byteBuffer = Encoding.Default.GetBytes(data);
        var randomFormat = ConvertToRandomFormat(byteBuffer);
        var cipherText = EncodeAES256CBC(byteBuffer, this.Aes256CbcKey, this.Aes256CbcInitialVector);
        return string.Concat(cipherText.Select(b => b.ToString("x2")));
    }

    /// <summary>
    /// Create random information based on datetime
    /// </summary>
    /// <returns></returns>
    private static byte[] GetRandomInformationByDatetime() 
    {
        var randomString = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fffffff");
        var randomBytes = Encoding.Default.GetBytes(randomString);
        var sha384 = SHA384.Create();
        for(int i=0; i<DateTime.Now.Month; i++)
            randomBytes = sha384.ComputeHash(randomBytes);

        return randomBytes;
    }

    /// <summary>
    /// this function use year and month information to
    /// create new combination key
    /// </summary>
    /// <param name="baseInfo"></param>
    /// <returns></returns>
    public static byte[] Generate256BitsKeyByDatetime(byte[] baseInfo) 
    {
        try
        {
            if (baseInfo == null || !baseInfo.Any()) throw new Exception("baseInfo cannot be null or empty");

            var _base = new byte[baseInfo.Length];
            Array.Copy(baseInfo, _base, _base.Length);

            var datetimeInfo = GetRandomInformationByDatetime();
            var combined = new byte[_base.Length + datetimeInfo.Length];
            Array.Copy(_base, 0, combined, 0, _base.Length);
            Array.Copy(datetimeInfo, 0, combined, _base.Length, datetimeInfo.Length);

            var combinedHash = SHA256.Create().ComputeHash(combined);

            return combinedHash;
        }
        catch (Exception ex)
        {
            throw new Exception("Error on generate key by datetime");
        }
    }

    /// <summary>
    /// this function use year and month information to
    /// create new combination initial vector
    /// </summary>
    /// <param name="baseInfo"></param>
    /// <returns></returns>
    public static byte[] GenerateInitialVectorByDatetime(byte[] baseInfo)
    {
        try
        {
            if (baseInfo == null || !baseInfo.Any()) throw new Exception("baseInfo cannot be null or empty");

            var _base = new byte[baseInfo.Length];
            Array.Copy(baseInfo, _base, _base.Length);

            var datetimeInfo = GetRandomInformationByDatetime();
            var combined = new byte[_base.Length + datetimeInfo.Length];
            Array.Copy(_base, 0, combined, 0, _base.Length);
            Array.Copy(datetimeInfo, 0, combined, _base.Length, datetimeInfo.Length);

            var combinedHash = MD5.Create().ComputeHash(combined);

            return combinedHash;
        }
        catch (Exception ex)
        {
            throw new Exception("Error on generate key by datetime");
        }
    }

    /// <summary>
        /// Encode byte data into cipher data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
    public static byte[] EncodeAES256CBC(byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
    {
        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.KeySize = 256;
        aes.Key = key;
        aes.IV = initialVector;

        var core = aes.CreateEncryptor(aes.Key, aes.IV);

        byte[] cipher;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, core, CryptoStreamMode.Write))
            {
                cs.Write(value, 0, value.Length);
                cs.FlushFinalBlock();
            }
            cipher = ms.ToArray();
        }

        return rounds == 0 ? cipher : EncodeAES256CBC(cipher, aes.Key, aes.IV, rounds - 1);
    }

    /// <summary>
    /// Compute Aes decryption
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <param name="initialVector"></param>
    /// <returns></returns>
    public static byte[] DecodeAES256CBC(byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = initialVector;

            var core = aes.CreateDecryptor();

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, core, CryptoStreamMode.Write))
                {
                    cs.Write(value, 0, value.Length);
                    cs.FlushFinalBlock();
                }
                return rounds == 0 ? ms.ToArray() : DecodeAES256CBC(ms.ToArray(), aes.Key, aes.IV, rounds - 1);
            }
        }

    /// <summary>
    /// Convert byte data to combined random info based on datetime
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] ConvertToRandomFormat(byte[] value) 
    {
        try
        {
            var randomInfo = GetRandomInformationByDatetime();
            var combined = new byte[randomInfo.Length + value.Length];

            Array.Copy(value, combined, value.Length);
            Array.Copy(randomInfo, 0, combined, value.Length, randomInfo.Length);

            return combined;
        }
        catch
        {
            throw new Exception("Error on convert to random format");
        }
    }

    /// <summary>
    /// Convert byte data from combined random info based on datetime
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] ConvertFromRandomFormat(byte[] value) 
    {
        try
        {
            var sizeBase = GetRandomInformationByDatetime().Length;
            var originalInfo = new byte[value.Length - sizeBase];

            Array.Copy(value, originalInfo, originalInfo.Length);

            return originalInfo;
        }
        catch
        {
            throw new Exception("Error on convert from random format");
        }
    }

    public Cryptography(string baseInfo){
        if(string.IsNullOrEmpty(baseInfo))
            throw new Exception("BaseInfo cannot be null or empty");

        var baseBuffer = Encoding.Default.GetBytes(baseInfo);
        this.Aes256CbcKey = Generate256BitsKeyByDatetime(baseBuffer);
        this.Aes256CbcInitialVector = GenerateInitialVectorByDatetime(baseBuffer);
    }
}
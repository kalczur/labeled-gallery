using System.Security.Cryptography;

namespace LabeledGallery.Utils;

public class HashUtils
{
    public static string GetStringSha256Hash(string text)
    {
        using (var algorithm  = SHA256.Create())
        {
            var textData = System.Text.Encoding.UTF8.GetBytes(text);
            var hash = algorithm .ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
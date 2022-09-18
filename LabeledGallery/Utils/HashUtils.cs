using System.Security.Cryptography;
using System.Text;

namespace LabeledGallery.Utils;

public class HashUtils
{
    public static string GetStringSha256Hash(string? text)
    {
        using (var algorithm = SHA256.Create())
        {
            var textData = Encoding.UTF8.GetBytes(text);
            var hash = algorithm.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
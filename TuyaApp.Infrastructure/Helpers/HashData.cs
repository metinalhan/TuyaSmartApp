using System.Security.Cryptography;
using System.Text;

namespace TuyaApp.Infrastructure.Helpers
{
    public static class HashData
    {
        // Calculates the HMACSHA256 hash of the input data using the provided secret key
        public static string Hash(string data, string secret)
        {
            // Convert the data and secret to byte arrays
            var dataAsBytes = Encoding.UTF8.GetBytes(data);
            var secretDataAsByte = Encoding.UTF8.GetBytes(secret);

            // Create a new HMACSHA256 instance with the secret key
            using (var hasher = new HMACSHA256(secretDataAsByte))
            {
                // Compute the hash of the data and return it as a hexadecimal string
                return ToHexString(hasher.ComputeHash(dataAsBytes)).ToUpper();
            }
        }

        // Converts a byte array to a hexadecimal string
        private static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}

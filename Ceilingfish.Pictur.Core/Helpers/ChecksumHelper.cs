using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ceilingfish.Pictur.Core.Helpers
{
    internal class ChecksumHelper
    {
        internal static string GetMd5HashFromFile(string fileName)
        {
            var file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(file);
            file.Close();

            var sb = new StringBuilder();
            for (var i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace UserMgr.Domain
{
    public class HashHelper
    {
        public static string ComputeMd5Hash(string password)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                return Encoding.ASCII.GetString(result);
            }
        }
    }
}

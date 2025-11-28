using System.Security.Cryptography;
using System.Text;

namespace Blogs.Service.Helpers
{
    public static class PasswordHelper
    {
        public static string GetPasswordHesh(string password)
        {
            MD5 MD5Hash = MD5.Create(); 
            byte[] inputBytes = Encoding.ASCII.GetBytes(password); 
            byte[] hash = MD5Hash.ComputeHash(inputBytes); 
            return Convert.ToHexString(hash);
        }
    }
}

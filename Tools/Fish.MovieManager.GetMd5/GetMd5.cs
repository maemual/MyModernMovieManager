using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fish.MovieManager.GetMd5
{
    public class GetMd5
    {
        private static GetMd5 _instance = new GetMd5();
        public static GetMd5 Instance { get { return _instance; } }

        public string Get(string path)
        {
            if (!File.Exists(path)) return string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                System.Security.Cryptography.HashAlgorithm md5 = System.Security.Cryptography.MD5.Create();
                return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "");
            }
        }
    }
}

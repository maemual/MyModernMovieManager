using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Fish.MovieManager.GetFile
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        /// <summary>
        /// 根据URL下载文件到指定path下
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="path">文件保存的路径及名称</param>
        public void GetFileFromWeb(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, path);
        }

        /// <summary>
        /// 从网址的最后获得名称
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>文件名</returns>
        public string GetFileNameFromUrl(string url)
        {
            int pos;
            for (pos = url.Length - 1; pos >= 0; pos--)
            {
                if (url[pos] == '/')
                    return url.Substring(pos + 1);
            }
            return null;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的MD5值</returns>
        public string GetFileMd5(string path)
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

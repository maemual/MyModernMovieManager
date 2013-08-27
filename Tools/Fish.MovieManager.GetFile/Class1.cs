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

        public void GetFileFromWeb(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, path);
        }

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
    }
}

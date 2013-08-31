using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fish.MovieManager;

namespace Fish.MovieManager.DoubanAPI
{
    public class Class
    {
        private static Class _instance = new Class();
        public static Class Instance { get { return _instance; } }

        public int GetSearchResult(string keyword)
        {
            string url = String.Format("https://api.douban.com/v2/movie/search?q={0}&count=1", keyword);
            
            var json = JObject.Parse(GetJson(url));
            Console.WriteLine(json);
            return int.Parse((String)json["subjects"][0]["id"]);
        }

        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GetMovieInfo(string name)
        {
			var movie = new Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo();

            return movie;
        }
        private string GetJson(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream recvStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(recvStream, Encoding.UTF8);

            String result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return result;
        }
    }
}

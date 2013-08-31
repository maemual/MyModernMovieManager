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

<<<<<<< HEAD
=======
        /// <summary>
        /// 根据关键词从豆瓣上搜索结果
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>返回搜索的第一个结果的豆瓣ID号</returns>
>>>>>>> maemual/master
        public int GetSearchResult(string keyword)
        {
            string url = String.Format("https://api.douban.com/v2/movie/search?q={0}&count=1", keyword);
            
            var json = JObject.Parse(GetJson(url));
<<<<<<< HEAD
            Console.WriteLine(json);
            return int.Parse((String)json["subjects"][0]["id"]);
        }

        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GetMovieInfo(string name)
        {
			var movie = new Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo();

            return movie;
        }
=======
            return int.Parse((String)json["subjects"][0]["id"]);
        }

        /// <summary>
        /// 根据电影名称，利用豆瓣的API抓取其主要信息
        /// </summary>
        /// <param name="name">电影的名称</param>
        /// <returns>一个DoubanMovieInfo类型</returns>
        public DoubanMovieInfo.Storage.DoubanMovieInfo GetMovieInfo(string name)
        {
            var movie = new DoubanMovieInfo.Storage.DoubanMovieInfo();
            var doubanId = GetSearchResult(name);

            string url = String.Format("https://api.douban.com/v2/movie/subject/{0}", doubanId);
            var json = JObject.Parse(GetJson(url));

            movie.doubanId = int.Parse((string)json["id"]);
            movie.title = (string)json["title"];
            if (json.GetValue("original_title") != null)
            {
                movie.originalTitle = (string)json["original_title"];
            }
            if (json.GetValue("aka") != null)
            {
                List<string> lst = new List<string>();
                foreach (var item in json["aka"].ToList())
                {
                    lst.Add((string)item);
                }
                movie.aka = string.Join("/", lst);
            }
            if (json.GetValue("rating") != null)
            {
                movie.rating = float.Parse((string)json["rating"]["average"]);
            }
            if (json.GetValue("ratings_count") != null)
            {
                movie.ratingsCount = int.Parse((string)json["ratings_count"]);
            }
            if (json.GetValue("images") != null)
            {
                var getFile = Fish.MovieManager.GetFile.Class1.Instance;
                string image_url = (string)json["images"]["large"];
                string image_name = GetFile.Class1.Instance.GetFileNameFromUrl(image_url);
                string path = String.Format("{0}\\movie_images\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, image_name);
                movie.image = path;

                Fish.MovieManager.GetFile.Class1.Instance.GetFileFromWeb(image_url, path);
            }
            if (json.GetValue("directors") != null)
            {
                movie.directors = int.Parse((string)json["directors"][0]["id"]);
            }
            if (json.GetValue("casts") != null)
            {
                foreach (var item in json["casts"].ToList())
                {
                    movie.casts.Add(int.Parse((string)item["id"]));
                }
            }
            if (json.GetValue("douban_site") != null)
            {
                movie.doubanSite = (string)json["douban_site"];
            }
            if (json.GetValue("year") != null)
            {
                movie.year = int.Parse((string)json["year"]);
            }
            if (json.GetValue("genres") != null)
            {
                foreach (var item in json["genres"].ToList())
                {
                    movie.genres.Add((string)item);
                }
            }
            if (json.GetValue("countries") != null)
            {
                List<string> lst = new List<string>();
                foreach (var item in json["countries"].ToList())
                {
                    lst.Add((string)item);
                }
                movie.countries = string.Join("/", lst);
            }
            if (json.GetValue("summary") != null)
            {
                movie.summary = (string)json["summary"];
            }
            return movie;
        }

        /// <summary>
        /// 根据影人ID号来从豆瓣抓取数据
        /// </summary>
        /// <param name="id">影人ID号</param>
        /// <returns>返回影人的DoubanActorInfo类型</returns>
        public DoubanActorInfo.Storage.DoubanActorInfo GetActorInfo(int id)
        {
            var actor = new DoubanActorInfo.Storage.DoubanActorInfo();
            string url = string.Format("https://api.douban.com/v2/movie/celebrity/{0}", id);
            var json = JObject.Parse(GetJson(url));

            Console.WriteLine(json);
            actor.id = int.Parse((string)json["id"]);
            if (json.GetValue("name") != null)
            {
                actor.name = (string)json["name"];
            }
            if (json.GetValue("name_en") != null)
            {
                actor.nameEn = (string)json["name_en"];
            }
            if (json.GetValue("gender") != null)
            {
                actor.gender = (string)json["gender"];
            }
            if (json.GetValue("born_place") != null)
            {
                actor.bornPlace = (string)json["born_place"];
            }
            if (json.GetValue("avatars") != null)
            {
                var getFile = Fish.MovieManager.GetFile.Class1.Instance;
                string image_url = (string)json["avatars"]["large"];
                string image_name = GetFile.Class1.Instance.GetFileNameFromUrl(image_url);
                string path = String.Format("{0}\\avatars\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, image_name);
                actor.avatars = path;

                Fish.MovieManager.GetFile.Class1.Instance.GetFileFromWeb(image_url, path);
            }
            return actor;
        }

        /// <summary>
        /// 根据上层构造的豆瓣API的URL，来获取豆瓣响应的JSON字符串
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>豆瓣上响应的JSON字符串</returns>
>>>>>>> maemual/master
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

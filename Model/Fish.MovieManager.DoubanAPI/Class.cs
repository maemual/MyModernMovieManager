﻿using System;
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
        /// <summary>
        /// 根据关键词从豆瓣上搜索结果
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>返回搜索的第一个结果的豆瓣ID号</returns>
        public int GetSearchResult(string keyword)
        {
            string url = String.Format("https://api.douban.com/v2/movie/search?q={0}&count=1", keyword);
            
            var json = JObject.Parse(GetJson(url));
            //Console.WriteLine(json);
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
            //TODO: aka
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
                string image_url = (string)json["images"]["medium"];
                string image_name = GetFile.Class1.Instance.GetFileNameFromUrl(image_url);
                string path = String.Format("{0}\\movie_images\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, image_name);
                movie.image = path;

                Fish.MovieManager.GetFile.Class1.Instance.GetFileFromWeb(image_url, image_url);
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
        /// 根据上层构造的豆瓣API的URL，来获取豆瓣响应的JSON字符串
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>豆瓣上响应的JSON字符串</returns>
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

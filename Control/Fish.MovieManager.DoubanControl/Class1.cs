﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Fish.MovieManager.DoubanMovieInfo;

namespace Fish.MovieManager.DoubanControl
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        /// <summary>
        /// 根据电影名称，从网络上抓取数据
        /// </summary>
        /// <param name="name">电影名称</param>
        /// <returns>DoubanMovieInfo类型</returns>
        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GetDoubanMovieInfo(string name)
        {
            var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(name);
            return movie;
        }

        /// <summary>
        /// 根据豆瓣ID从数据库中读取数据
        /// </summary>
        /// <param name="id">豆瓣ID</param>
        /// <returns>DoubanMovieInfo类型</returns>
        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GeDoubanMovieInfo(int id)
        {
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var res = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == id).SingleOrDefault();
                return res;
            }
        }
    }
}
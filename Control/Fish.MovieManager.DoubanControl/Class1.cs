using System;
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
        private static Class1 _instance;
        public static Class1 Instance {
            get 
            {
                if (_instance == null)
                {
                    _instance = new Class1();
                }
                return _instance; 
            } 
        }

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
        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GetDoubanMovieInfo(int id)
        {
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var res = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == id).SingleOrDefault();
                return res;
            }
        }

        /// <summary>
        /// 添加豆瓣电影信息到数据库中
        /// </summary>
        /// <param name="movie">豆瓣DoubanMovieInfo类型</param>
        public void AddDoubanMovieInfo(Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo movie)
        {
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var tmp = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == movie.doubanId).SingleOrDefault();
                if (tmp == null)
                {
                    try
                    {
                        session.Save(movie);
                        session.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        session.Transaction.Rollback();
                        throw new Exception("wrong storage.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 获取导演名称
        /// </summary>
        /// <param name="doubanId">豆瓣ID</param>
        /// <returns>导演名称</returns>
        public string GetDirectorName(int doubanId)
        {
            int dirctor = 0;
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == doubanId).SingleOrDefault();
                if (tmp != null)
                {
                    dirctor = tmp.directors;
                }
            }
            string ans = "";
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == dirctor).SingleOrDefault();
                if (tmp != null)
                {
                    ans = tmp.name;
                }
            }
            return ans;
        }

        /// <summary>
        /// 获取导演信息
        /// </summary>
        /// <param name="doubanID">导演的豆瓣ID</param>
        /// <returns>ActorInfo类</returns>
        public Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo GetDirectorInfo(int doubanID)
        {
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == doubanID).SingleOrDefault();
                return tmp;
            }
        }

        /// <summary>
        /// 获取一部电影的类型信息
        /// </summary>
        /// <param name="doubanID">豆瓣ID</param>
        /// <returns>类型信息</returns>
        public string GetMovieTag(int doubanID)
        {
            string ret = "";
            List<string> lst = new List<string>();
            using (var session = Fish.MovieManager.Movie2Tag.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag>().Where(o => o.id == doubanID).Select(o => o.tag).ToList();
                if (tmp != null)
                {
                    foreach (var item in tmp)
                    {
                        lst.Add(item);
                    }
                }
            }
            ret = string.Join("/", lst);
            return ret;
        }

        /// <summary>
        /// 获取演员名称
        /// </summary>
        /// <param name="doubanID">豆瓣ID</param>
        /// <returns>演员名称，由“/”连接</returns>
        public string GetActorName(int doubanID)
        {
            string ret = "";
            List<int> id_list = new List<int>();
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.id == doubanID).Select(o => o.doubanId).ToList();
                foreach (var item in tmp)
                {
                    id_list.Add(item);
                }
            }
            List<string> name_list = new List<string>();
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                foreach(var id in id_list)
                {
                    var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == id).Select(o => o.name).SingleOrDefault();
                    name_list.Add(tmp);
                }
            }
            ret = string.Join("/", name_list);
            return ret;
        }
    }
}

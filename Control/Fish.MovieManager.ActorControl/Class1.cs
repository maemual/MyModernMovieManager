using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace Fish.MovieManager.ActorControl
{
    public class Class1
    {
        private static Class1 _instance;
        public static Class1 Instance { 
            get {
                if (_instance == null)
                {
                    _instance = new Class1();
                }
                return _instance; 
            } 
        }

        /// <summary>
        /// 根据豆瓣ID，获取其所有主演信息
        /// </summary>
        /// <param name="doubanId">豆瓣ID</param>
        /// <returns>演员信息的List</returns>
        public List<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo> GetActorByID(int doubanId)
        {
            var ans = new List<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>();
            var id = new List<int>();
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.id == doubanId).Select(o => o.doubanId).ToList();
                foreach (var item in tmp)
                {
                    id.Add(item);
                }
            }
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                foreach(var item in id)
                {
                    var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == item).SingleOrDefault();
                    if (tmp != null)
                    {
                        ans.Add(tmp);
                    }
                }
            }
            return ans;
        }

        /// <summary>
        /// 根据一个演员的豆瓣ID，从豆瓣抓取信息，添加到数据库中
        /// </summary>
        /// <param name="id">演员的豆瓣ID</param>
        public void AddActorInfo(int id)
        {
            var actor = Fish.MovieManager.DoubanAPI.Class.Instance.GetActorInfo(id);
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == actor.id).SingleOrDefault();
                if (tmp == null)
                {
                    try
                    {
                        session.Save(actor);
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
        /// 添加一个电影的演员对应关系
        /// </summary>
        /// <param name="m2a">Movie2Actor类型</param>
        public void AddActor(Fish.MovieManager.Movie2Actor.Storage.Movie2Actor m2a)
        {
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var tmp = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.id == m2a.id && o.doubanId == m2a.doubanId).SingleOrDefault();
                if (tmp == null)
                {
                    try 
                    {
                        session.Save(m2a);
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
    }
}

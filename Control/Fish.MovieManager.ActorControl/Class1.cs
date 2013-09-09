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
        /// <summary>
        /// 根据豆瓣ID，获取其所有主演信息
        /// 现在有缺陷，会返回所有信息，包括导演，需要自己根据导演ID筛选一下。
        /// </summary>
        /// <param name="doubanId">豆瓣ID</param>
        /// <returns>演员信息的List</returns>
        public List<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo> GetActorByID(int doubanId)
        {
            var ans = new List<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>();
            var id = new List<int>();
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.id == doubanId).ToList();
                foreach (var item in tmp)
                {
                    id.Add(item.doubanId);
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
    }
}

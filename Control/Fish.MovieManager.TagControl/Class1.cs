using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace Fish.MovieManager.TagControl
{
    public class Class1
    {
        /// <summary>
        /// 获取一部电影的标签
        /// </summary>
        /// <param name="doubanId">豆瓣ID</param>
        /// <returns>包含电影标签的List</returns>
        public List<string> GetMovieTag(int doubanId)
        {
            var ans = new List<string>();
            using (var session = Fish.MovieManager.Movie2Tag.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag>().Where(o => o.id == doubanId).ToList();
                if (tmp != null)
                {
                    foreach (var item in tmp)
                    {
                        ans.Add(item.tag);
                    }
                }
            }
            return ans;
        }

        /// <summary>
        /// 由标签，获取所有电影的信息
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns>电影列表</returns>
        public List<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo> GetMovieByTag(string tag)
        {
            var ans = new List<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>();
            var douban = new List<int>();
            using (var session = Fish.MovieManager.Movie2Tag.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag>().Where(o => o.tag == tag).ToList();
                foreach (var item in tmp)
                {
                    douban.Add(item.id);
                }
            }
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                foreach (var item in douban)
                {
                    var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.doubanId == item).SingleOrDefault();
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

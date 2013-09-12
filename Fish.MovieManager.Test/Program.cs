using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fish.MovieManager.Movie2Actor.Storage;
using NHibernate.Linq;

namespace Fish.MovieManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
			//using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
			//{
			//	var video = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == 286).SingleOrDefault();
			//	Console.WriteLine(video.path);
			//}
			Fish.MovieManager.MovieControl.Class1.Instance.ImportFiles("D:\\电影 mv\\test");
			//var tmp = Fish.MovieManager.DoubanAPI.Class.Instance;
			//Console.WriteLine(tmp.GetSearchResult("杀死比尔"));
            //var tmp = Fish.MovieManager.GetMd5.GetMd5.Instance;
            //Console.WriteLine(tmp.Get("D:\\Pictures\\z01.png"));
            //using (var session = StorageManager.Instance.OpenSession())
            //{
            //    //var tmp = new Fish.MovieManager.Movie2Actor.Storage.Movie2Actor();
            //    //tmp.id = 1;
            //    //tmp.doubanId = 4;
                
            //    //try
            //    //{
            //    //    session.BeginTransaction();

            //    //    session.Save(tmp);
            //    //    session.Transaction.Commit();
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    session.Transaction.Rollback();
            //    //    throw new Exception("失败", ex);
            //    //}
            //}
        }
    }
}

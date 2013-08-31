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
            var tmp = Fish.MovieManager.DoubanAPI.Class.Instance;
            Console.WriteLine(tmp.GetSearchResult("杀死比尔"));
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

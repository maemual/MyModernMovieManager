using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fish.MovieManager.DoubanMovieInfo.Storage;
using NHibernate.Linq;

namespace Fish.MovieManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = StorageManager.Instance.OpenSession())
            {
                var tmp = new DoubanMovieInfo.Storage.DoubanMovieInfo();
                tmp.aka = "aka";
                tmp.countries = "countries";
                tmp.directors = 1;
                tmp.doubanId = 2;
                tmp.doubanSite = "doubanSite";
                tmp.image = "image";
                tmp.originalTitle = "originalTitle";
                tmp.rating = (double)3.4;
                tmp.ratingsCount = 3;
                tmp.summary = "summary";
                tmp.title = "title";
                tmp.year = 4;
            
                
                try
                {
                    session.BeginTransaction();

                    session.Save(tmp);
                    session.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new Exception("失败", ex);
                }
            }
        }
    }
}

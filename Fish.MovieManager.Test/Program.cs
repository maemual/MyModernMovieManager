using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fish.MovieManager.DoubanActorInfo.Storage;
using NHibernate.Linq;

namespace Fish.MovieManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = StorageManager.Instance.OpenSession())
            {
                var tmp = new Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo();
                tmp.avatars = "avatars";
                tmp.bornPlace = "bornPlace";
                tmp.gender = 'f';
                tmp.id = 123332;
                tmp.name = "name";
                tmp.nameEn = "nameEn";
                
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

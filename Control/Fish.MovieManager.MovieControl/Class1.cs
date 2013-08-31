using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Fish.MovieManager.VideoFileInfo;

namespace Fish.MovieManager.MovieControl
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        public void ImportFiles(string path)
        {
            List<string> files = Fish.MovieManager.GetFile.Class1.Instance.GetFilesFromPath(path);

            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                foreach (var item in files)
                {
                    var file = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo(item);
                    file.path = item;
                        try
                        {
                            var tmp = session.Query<VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.path == item).SingleOrDefault();
                            if (tmp == null)
                            {
                                //Console.WriteLine("OK？");
                                session.Save(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            session.Transaction.Rollback();
                            throw new Exception("stroage wrong", ex);
                        }
                }
                session.Transaction.Commit();
            }
        }
    }
}

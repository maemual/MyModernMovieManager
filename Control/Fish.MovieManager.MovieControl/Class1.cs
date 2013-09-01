using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Fish.MovieManager.VideoFileInfo;

namespace Fish.MovieManager.VideoControl
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        /// <summary>
        /// 导入路径下所有文件的信息
        /// </summary>
        /// <param name="path">路径</param>
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

        /// <summary>
        /// 根据文件ID获取一个视频
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns>VideoFileInfo类</returns>
        public Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo GetFileInfo(int id)
        {
            using(var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var video = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == id).SingleOrDefault();
                return video;
            }
        }

        /// <summary>
        /// 获取数据库中所有文件信息
        /// </summary>
        /// <returns>文件信息的列表</returns>
        public List<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo> GetAllFileInfo()
        {
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().ToList();
                return tmp;
            }
        }
    }
}

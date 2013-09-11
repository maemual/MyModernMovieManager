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
            //导入视频文件信息
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
            ImportDoubanInfo(files);
        }

        /// <summary>
        /// 导入豆瓣信息
        /// </summary>
        /// <param name="files">文件路径</param>
        public void ImportDoubanInfo(List<string> files)
        {
            //导入电影信息
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                foreach (var item in files)
                {
                    var fileName = System.IO.Path.GetFileName(item);
                    var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(fileName);
                    var tmp = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == movie.doubanId).SingleOrDefault();
                    if (tmp == null)
                    {
                        try
                        {
                            session.Save(movie);
                        }
                        catch (Exception ex)
                        {
                            session.Transaction.Rollback();
                            throw new Exception("wrong storage.", ex);
                        }
                        List<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag> tag = new List<Movie2Tag.Storage.Movie2Tag>();
                        foreach (var tags in movie.genres)
                        {
                            var t = new Fish.MovieManager.Movie2Tag.Storage.Movie2Tag();
                            t.id = movie.doubanId;
                            t.tag = tags;
                            //Console.WriteLine(t.tag);
                            tag.Add(t);
                        }
                        ImportMovieTag(tag);

                        List<int> person = new List<int>();
                        foreach (var p in movie.casts)
                        {
                            person.Add(p);
                        }
                        person.Add(movie.directors);
                        List<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor> actor = new List<Movie2Actor.Storage.Movie2Actor>();
                        foreach (var act in person)
                        {
                            var t = new Fish.MovieManager.Movie2Actor.Storage.Movie2Actor();
                            t.doubanId = movie.doubanId;
                            t.id = act;
                        }
                        ImportActor(actor);
                        ImportActorInfo(person);
                    }
                }
                session.Transaction.Commit();
            }
        }

        /// <summary>
        /// 导入电影类型信息
        /// </summary>
        /// <param name="lst">Movie2Tag类型的list</param>
        public void ImportMovieTag(List<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag> lst)
        {
            using (var session = Fish.MovieManager.Movie2Tag.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                foreach (var item in lst)
                {
                    //Console.WriteLine(item.id + "  " + item.tag);
                    var tmp = session.Query<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag>().Where(o => o.id == item.id && o.tag == item.tag).SingleOrDefault();
                    if (tmp == null)
                    {
                        try
                        {
                            //session.Save(item);
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

        public void ImportActorInfo(List<int> lst)
        {
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                foreach (var item in lst)
                {
                    var actor = Fish.MovieManager.DoubanAPI.Class.Instance.GetActorInfo(item);
                    var tmp = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == actor.id).SingleOrDefault();
                    if (tmp == null)
                    {
                        try
                        {
                            session.Save(actor);
                        }
                        catch (Exception ex)
                        {
                            session.Transaction.Rollback();
                            throw new Exception("wrong storage.", ex);
                        }
                    }
                }
                session.Transaction.Commit();
            }
        }

        public void ImportActor(List<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor> lst)
        {
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                foreach (var item in lst)
                {
                    var tmp = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.doubanId == item.doubanId && o.id == item.id).SingleOrDefault();
                    try
                    {
                        if (tmp == null)
                        {
                            session.Save(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        session.Transaction.Rollback();
                        throw new Exception("wrong storage.", ex);
                    }
                }
                session.Transaction.Commit();
            }
        }
        /// <summary>
        /// 根据文件的路径添加一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public bool ImportFile(string path)
        {
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var file = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo(path);
                file.path = path;
                try
                {
                    var tmp = session.Query<VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.path == path).SingleOrDefault();
                    if (tmp == null)
                    {
                        session.Save(file);
                        session.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new Exception("storage wrong", ex);
                }
            }
            return true;
        }

        public bool DeleteFile(int id)
        {
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == id).SingleOrDefault();
                session.BeginTransaction();
                try
                {
                    if (tmp != null)
                    {
                        session.Delete(tmp);
                        session.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new Exception("delete wrong", ex);
                }
            }
            return true;
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

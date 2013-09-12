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
            List<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo> doubanMoives = new List<DoubanMovieInfo.Storage.DoubanMovieInfo>();
            //导入视频文件信息
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                foreach (var item in files)
                {
                    var file = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo(item);
                    file.path = item;
                    var fileName = System.IO.Path.GetFileName(file.path);
                    var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(fileName);
                    doubanMoives.Add(movie);
                    file.doubanId = movie.doubanId;
                    try
                    {
                        session.BeginTransaction();
                        var tmp = session.Query<VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.path == item).SingleOrDefault();
                        if (tmp == null)
                        {
                            //Console.WriteLine("OK？");
                            session.Save(file);
                            session.Transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        session.Transaction.Rollback();
                        throw new Exception("stroage wrong", ex);
                    }
                }
            }
            ImportDoubanInfo(doubanMoives);
        }

        /// <summary>
        /// 导入豆瓣信息列表
        /// </summary>
        /// <param name="doubanMovies">DoubanMovieInfo类型的List</param>
        public void ImportDoubanInfo(List<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo> doubanMovies)
        {
            List<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag> tag = new List<Movie2Tag.Storage.Movie2Tag>();
            List<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor> actor = new List<Movie2Actor.Storage.Movie2Actor>();
            using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            {
                foreach (var item in doubanMovies)
                {
                    session.BeginTransaction();
                    var tmp = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == item.doubanId).SingleOrDefault();
                    if (tmp == null)
                    {
                        foreach (var tags in item.genres)
                        {
                            var t = new Fish.MovieManager.Movie2Tag.Storage.Movie2Tag();
                            t.id = item.doubanId;
                            t.tag = tags;
                            tag.Add(t);
                        }

                        foreach (var act in item.casts)
                        {
                            var t = new Fish.MovieManager.Movie2Actor.Storage.Movie2Actor();
                            t.id = item.doubanId;
                            t.doubanId = act;
                            actor.Add(t);
                        }
                        
                        using (var s = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
                        {
                            s.BeginTransaction();
                            var tt = s.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == item.directors).SingleOrDefault();
                            if (tt == null)
                            {
                                try
                                {
                                    var dir = Fish.MovieManager.DoubanAPI.Class.Instance.GetActorInfo(item.directors);
                                    s.Save(dir);
                                    s.Transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    s.Transaction.Rollback();
                                    throw new Exception("wrong storage.", ex);
                                }
                            }
                        }

                        try
                        {
                            session.Save(item);
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
            //TO-DO
            ImportMovie2Tag(tag);
            ImportActor(actor);
            ImportActorInfo(actor);
        }

        /// <summary>
        /// 由电影标签列表添加到数据库中
        /// </summary>
        /// <param name="lst"></param>
        public void ImportMovie2Tag(List<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag> lst)
        {
            using (var session = Fish.MovieManager.Movie2Tag.Storage.StorageManager.Instance.OpenSession())
            {
                foreach (var item in lst)
                {
                    Console.WriteLine(item.id + "  " + item.tag);
                    session.BeginTransaction();
                    var qs = session.Query<Fish.MovieManager.Movie2Tag.Storage.Movie2Tag>()
                        .Where(o => o.id == item.id && o.tag == item.tag).SingleOrDefault();
                    if (qs == null)
                    {
                        try
                        {
                            session.Clear();
                            session.Save(item);
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


        public void ImportActor(List<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor> lst)
        {
            using (var session = Fish.MovieManager.Movie2Actor.Storage.StorageManager.Instance.OpenSession())
            {
                foreach (var item in lst)
                {
                    Console.WriteLine(item.id + "  " + item.doubanId);
                    session.BeginTransaction();
                    var qs = session.Query<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor>().Where(o => o.doubanId == item.doubanId && o.id == item.id).SingleOrDefault();
                    if (qs == null)
                    {
                        try
                        {
                            session.Clear();
                            session.Save(item);
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

        public void ImportActorInfo(List<Fish.MovieManager.Movie2Actor.Storage.Movie2Actor> lst)
        {
            using (var session = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            {

                foreach (var item in lst)
                {
                    session.BeginTransaction();
                    var qs = session.Query<Fish.MovieManager.DoubanActorInfo.Storage.DoubanActorInfo>().Where(o => o.id == item.doubanId).SingleOrDefault();
                    if (qs == null)
                    {
                        var actor = Fish.MovieManager.DoubanAPI.Class.Instance.GetActorInfo(item.doubanId);
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
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
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

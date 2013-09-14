using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Net;
using System.IO;

namespace Fish.MovieManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tmp = Fish.MovieManager.TagControl.Class1.Instance.GetMovieByTag("剧情");
            foreach (var item in tmp)
            {
                Console.WriteLine(item.doubanId);
            }
            //var tmp = Fish.MovieManager.ActorControl.Class1.Instance.GetActorByID(1292000);
            //foreach (var item in tmp)
            //{
            //    Console.WriteLine(item.name);
            //}
            //Console.WriteLine(Fish.MovieManager.DoubanControl.Class1.Instance.GetActorName(1292000));
            
            //Fish.MovieManager.VideoControl.Class1.Instance.SetUserStar(667, 5);
            //Fish.MovieManager.VideoControl.Class1.Instance.ImportFile("D:\\Videos\\楚门的世界.rmvb");
            //var tmp = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo("陆小凤传奇之陆小凤前传");
            //Console.WriteLine(tmp.genres);
            //using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            //{
            //    session.BeginTransaction();
            //    var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == 283).SingleOrDefault();
            //    if (tmp != null)
            //    {
            //        session.Delete(tmp);
            //        session.Transaction.Commit();
            //    }
            //}
            //using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            //{
            //    var movie = Fish.MovieManager.DoubanControl.Class1.Instance.GetDoubanMovieInfo("七宗罪");
            //    session.BeginTransaction();
            //    session.Save(movie);
            //    session.Transaction.Commit();
            //}
            //var tmp = Fish.MovieManager.MovieControl.Class1.Instance.GetAllFileInfo();
            //foreach (var item in tmp)
            //{
            //    Console.WriteLine(item.path);
            //}
            //Fish.MovieManager.MovieControl.Class1.Instance.ImportFiles("D:\\Videos");
            //using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            //{
            //    //var video = new Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo();
            //    var video = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo("D:\\Videos\\Final.Fantasy.VII.Advent.Children.最终幻想VII.圣子降临(完整版).中文字幕.HR-HDTV.AC3.1024X576.x264-人人影视制作.mkv");
            //    try
            //    {
            //        session.BeginTransaction();
            //        session.Save(video);

            //        session.Transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        session.Transaction.Rollback();
            //        throw new Exception("bad", ex);
            //    }
            //}

            //Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
            //Fish.MovieManager.GetFile.Class1.Instance.GetFileFromWeb("http://img4.douban.com/mpic/s3018008.jpg", System.AppDomain.CurrentDomain.BaseDirectory + "\\movie_images\\" + Fish.MovieManager.GetFile.Class1.Instance.GetFileNameFromUrl("http://img4.douban.com/mpic/s3018008.jpg"));
            //var tmp = Fish.MovieManager.DoubanAPI.Class.Instance;
            //tmp.GetMovieInfo("杀死比尔");
            //var tmp = Fish.MovieManager.GetMd5.GetMd5.Instance;
            //Console.WriteLine(tmp.Get("D:\\Pictures\\z01.png"));

            //using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
            //{
            //    var m = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo("杀死比尔");

            //    try
            //    {
            //        session.BeginTransaction();
            //        session.Save(m);

            //        session.Transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        session.Transaction.Rollback();
            //        throw new Exception("bad", ex);
            //    }
            //    int id = m.directors;
            //    using (var s = Fish.MovieManager.DoubanActorInfo.Storage.StorageManager.Instance.OpenSession())
            //    {
            //        var d = Fish.MovieManager.DoubanAPI.Class.Instance.GetActorInfo(id);
            //        try
            //        {
            //            s.BeginTransaction();
            //            s.Save(d);

            //            s.Transaction.Commit();
            //        }
            //        catch (Exception e)
            //        {
            //            s.Transaction.Rollback();
            //            throw new Exception("some bad", e);
            //        }
            //    }
            //}
        }
    }
}

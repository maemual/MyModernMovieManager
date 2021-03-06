﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Fish.MovieManager.VideoFileInfo;
using System.Windows;

namespace Fish.MovieManager.VideoControl
{
    public class Class1
    {
        private static Class1 _instance;
        public static Class1 Instance 
        { 
            get 
            {
                if (_instance == null)
                {
                    _instance = new Class1();
                }
                return _instance; 
            } 
        }

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
                    var qs = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.path == item).SingleOrDefault();
                    if (qs != null) continue;
                    var file = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo(item);
                    file.path = item;
                    if (Fish.MovieManager.NetTest.Class1.Instance.NetTest())
                    {
                        var fileName = System.IO.Path.GetFileName(file.path);
                        var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(fileName);
                        doubanMoives.Add(movie);
                        file.doubanId = movie.doubanId;
                    }
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
            if (Fish.MovieManager.NetTest.Class1.Instance.NetTest())
            {
                ImportDoubanInfo(doubanMoives);
            }
        }

        /// <summary>
        /// 导入豆瓣信息列表
        /// </summary>
        /// <param name="doubanMovies">DoubanMovieInfo类型的List</param>
        public void ImportDoubanInfo(List<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo> doubanMovies)
        {
            foreach (var item in doubanMovies)
            {
                Fish.MovieManager.DoubanControl.Class1.Instance.AddDoubanMovieInfo(item);
                foreach (var t in item.genres)
                {
                    var tmp = new Fish.MovieManager.Movie2Tag.Storage.Movie2Tag();
                    tmp.id = item.doubanId;
                    tmp.tag = t;
                    Fish.MovieManager.TagControl.Class1.Instance.AddMovie2Tag(tmp);
                }

                foreach (var t in item.casts)
                {
                    var tmp = new Fish.MovieManager.Movie2Actor.Storage.Movie2Actor();
                    tmp.id = item.doubanId;
                    tmp.doubanId = t;
                    Fish.MovieManager.ActorControl.Class1.Instance.AddActor(tmp);
                    Fish.MovieManager.ActorControl.Class1.Instance.AddActorInfo(t);
                }
                Fish.MovieManager.ActorControl.Class1.Instance.AddActorInfo(item.directors);
            }
        }

        /// <summary>
        /// 根据文件的路径添加一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public void ImportFile(string path)
        {
            if (Fish.MovieManager.GetFile.Class1.Instance.isValidate(path) == false)
                return;

            List<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo> doubanMoives = new List<DoubanMovieInfo.Storage.DoubanMovieInfo>();
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                var qs = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.path == path).SingleOrDefault();
                if (qs != null) return;
                session.BeginTransaction();
                var file = Fish.MovieManager.VideoFileInfo.Class1.Instance.GetVideoFileInfo(path);
                file.path = path;
                if (Fish.MovieManager.NetTest.Class1.Instance.NetTest())
                {
                    var fileName = System.IO.Path.GetFileName(file.path);

                    var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(fileName);
                    doubanMoives.Add(movie);
                    file.doubanId = movie.doubanId;
                }
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
            if (Fish.MovieManager.NetTest.Class1.Instance.NetTest())
            {
                ImportDoubanInfo(doubanMoives);
            }
        }

        /// <summary>
        /// 刷新豆瓣信息
        /// </summary>
        /// <param name="id">豆瓣ID</param>
        public void RefreshInfo(int id)
        {
            //if (id != 0) return;
            var file = new Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo();
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                file = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == id).SingleOrDefault();
            }
            if (Fish.MovieManager.NetTest.Class1.Instance.NetTest())
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(file.path);
                var douban = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(fileName);
                file.doubanId = douban.doubanId;
                using (var session = Fish.MovieManager.DoubanMovieInfo.Storage.StorageManager.Instance.OpenSession())
                {
                    var tmp = session.Query<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>().Where(o => o.doubanId == douban.doubanId).SingleOrDefault();
                    if (tmp == null)
                    {
                        session.BeginTransaction();
                        try
                        {
                            session.Save(douban);
                            session.Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            session.Transaction.Rollback();
                            throw new Exception("wrong storage.", ex);
                        }
                    }
                }

                using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
                {
                    session.BeginTransaction();
                    try
                    {
                        session.SaveOrUpdate(file);
                        session.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        session.Transaction.Rollback();
                        throw new Exception("wrong storage.", ex);
                    }
                }
                var doubanlist = new List<Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo>();
                doubanlist.Add(douban);
                ImportDoubanInfo(doubanlist);
            }
        }

        /// <summary>
        /// 根据给定的ID，删除文件信息
        /// </summary>
        /// <param name="id">文件ID</param>
        public void DeleteFile(int id)
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

        /// <summary>
        /// 计算给定文件ID的MD5值
        /// </summary>
        /// <param name="id">文件ID</param>
        public string SetMd5(int id)
        {
            string md5;
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == id).SingleOrDefault();
                var path = tmp.path;
                if (tmp.md5 != null) return tmp.md5;
                md5 = Fish.MovieManager.GetFile.Class1.Instance.GetFileMd5(path);
                tmp.md5 = md5;

                try
                {
                    session.Update(tmp);
                    session.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new Exception("wrong storage.", ex);
                }
            }
            return md5;
        }

        /// <summary>
        /// 设置用户评分
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="star">星级</param>
        public void SetUserStar(int id, int star)
        {
            using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
            {
                session.BeginTransaction();
                var tmp = session.Query<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo>().Where(o => o.id == id).SingleOrDefault();
                if (tmp != null)
                {
                    tmp.userRating = star;
                    try
                    {
                        session.Update(tmp);
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
}

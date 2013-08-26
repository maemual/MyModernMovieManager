using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fish.MovieManager.VideoFileInfo.Storage;
using NHibernate.Linq;

namespace Fish.MovieManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = StorageManager.Instance.OpenSession())
            {
                var tmp = new Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo();
                tmp.audioBitRate = 1;
                tmp.audioFormat = "audio";
                tmp.bitRate = 2;
                tmp.doubanId = 3;
                tmp.duration = "duration";
                tmp.extension = "extension";
                tmp.frameRate = 4;
                tmp.height = 5;
                tmp.md5 = "md5";
                tmp.path = "path";
                tmp.totalFrames = 6;
                tmp.userRating = 7;
                tmp.videoBitRate = 8;
                tmp.videoFormat = "video";
                tmp.width = 9;
                
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

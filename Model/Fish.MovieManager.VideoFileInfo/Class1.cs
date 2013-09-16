using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.VideoFileInfo
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
        /// 获取一个视频文件的文件信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>VideoFileInfo类型,没有豆瓣ID</returns>
        public VideoFileInfo.Storage.VideoFileInfo GetVideoFileInfo(string path)
        {
            var video = new VideoFileInfo.Storage.VideoFileInfo();

            VideoEncoder.Encoder enc = new VideoEncoder.Encoder();
            enc.FFmpegPath = "ffmpeg.exe";

            VideoEncoder.VideoFile videoFile = new VideoEncoder.VideoFile(path);
            enc.GetVideoInfo(videoFile);

            System.TimeSpan totatp = videoFile.Duration;
            string totalTime = string.Format("{0:00}:{1:00}:{2:00}", (int)totatp.Hours, (int)totatp.Minutes, (int)totatp.Seconds);

            video.audioBitRate = (float)videoFile.AudioBitRate;
            video.audioFormat = videoFile.AudioFormat;
            video.bitRate = (float)videoFile.BitRate;
            video.duration = totalTime;
            video.extension = System.IO.Path.GetExtension(path).Substring(1);
            video.frameRate = (float)videoFile.FrameRate;
            video.height = videoFile.Height;
            video.path = path;
            video.totalFrames = videoFile.TotalFrames;
            video.userRating = -1;
            video.videoBitRate = (float)videoFile.VideoBitRate;
            video.videoFormat = videoFile.VideoFormat;
            video.width = videoFile.Width;

            return video;
        }

        /// <summary>
        /// 播放文件
        /// </summary>
        /// <param name="path"></param>
        public void PlayVideo(string path)
        {
            System.Diagnostics.Process ps = new System.Diagnostics.Process();
            ps.StartInfo.FileName = path;
            ps.Start();
        }
    }
}

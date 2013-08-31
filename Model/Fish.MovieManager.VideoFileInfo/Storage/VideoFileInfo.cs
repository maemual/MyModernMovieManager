using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.VideoFileInfo.Storage
{
    public class VideoFileInfo
    {
        /// <summary>
        /// 文件自增长ID
        /// </summary>
        public virtual int id { get; set; }
        /// <summary>
        /// 电影豆瓣ID
        /// </summary>
        public virtual int doubanId { get; set; }
        /// <summary>
        /// 文件存储路径
        /// </summary>
        public virtual string path { get; set; }
        /// <summary>
        /// 视频文件时长
        /// </summary>
        public virtual string duration { get; set; }
        /// <summary>
        /// 比特率
        /// </summary>
        public virtual float bitRate { get; set; }
        /// <summary>
        /// 视频比特率
        /// </summary>
        public virtual float videoBitRate { get; set; }
        /// <summary>
        /// 音频比特率
        /// </summary>
        public virtual float audioBitRate { get; set; }
        /// <summary>
        /// 音频编码格式
        /// </summary>
        public virtual string audioFormat { get; set; }
        /// <summary>
        /// 视频编码格式
        /// </summary>
        public virtual string videoFormat { get; set; }
        /// <summary>
        /// 帧速率
        /// </summary>
        public virtual float frameRate { get; set; }
        /// <summary>
        /// 文件拓展名
        /// </summary>
        public virtual string extension { get; set; }
        /// <summary>
        /// 视频高
        /// </summary>
        public virtual int height { get; set; }
        /// <summary>
        /// 视频宽
        /// </summary>
        public virtual int width { get; set; }
        /// <summary>
        /// 视频总帧数
        /// </summary>
        public virtual long totalFrames { get; set; }
        /// <summary>
        /// 用户的评分
        /// </summary>
        public virtual int userRating { get; set; }
        /// <summary>
        /// 文件的md5值
        /// </summary>
        public virtual string md5 { get; set; }
    }
}

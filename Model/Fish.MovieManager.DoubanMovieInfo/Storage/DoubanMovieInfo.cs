using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.DoubanMovieInfo.Storage
{
    public class DoubanMovieInfo
    {
        public DoubanMovieInfo()
        {
            casts = new List<int>();
            genres = new List<string>();
        }
        #region 数据库表属性
        /// <summary>
        /// 豆瓣上的条目ID
        /// </summary>
        public virtual int doubanId { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public virtual string title { get; set; }
        /// <summary>
        /// 原名
        /// </summary>
        public virtual string originalTitle { get; set; }
        /// <summary>
        /// 又名，中间通过 / 来连接多个又名
        /// </summary>
        public virtual string aka { get; set; }
        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public virtual double rating { get; set; }
        /// <summary>
        /// 豆瓣评分人数
        /// </summary>
        public virtual int ratingsCount { get; set; }
        /// <summary>
        /// 电影海报图存储的路径
        /// </summary>
        public virtual string image { get; set; }
        /// <summary>
        /// 导演，存储影人的豆瓣ID号
        /// </summary>
        public virtual int directors { get; set; }
        /// <summary>
        /// 豆瓣小站地址
        /// </summary>
        public virtual string doubanSite { get; set; }
        /// <summary>
        /// 年代
        /// </summary>
        public virtual int year { get; set; }
        /// <summary>
        /// 出品国家或地区
        /// </summary>
        public virtual string countries { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public virtual string summary { get; set; }
        /// <summary>
        /// 影评人的豆瓣ID号，不进行数据库映射
        /// </summary>
        public List<int> casts { get; set; }
        /// <summary>
        /// 电影的类型，不进行数据库的映射
        /// </summary>
        public List<string> genres { get; set; }
        #endregion
    }
}

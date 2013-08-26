using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.Movie2Tag.Storage
{
    public class Movie2Tag
    {
        /// <summary>
        /// 电影豆瓣ID
        /// </summary>
        public virtual int id { get; set; }
        /// <summary>
        /// 电影的类型
        /// </summary>
        public virtual string tag { get; set; }
    }
}

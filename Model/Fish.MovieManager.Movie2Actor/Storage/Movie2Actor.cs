using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.Movie2Actor.Storage
{
    public class Movie2Actor
    {
        /// <summary>
        /// 电影的豆瓣ID
        /// </summary>
        public virtual int id { get; set; }
        /// <summary>
        /// 电影中的影人的豆瓣ID
        /// </summary>
        public virtual int doubanId { get; set; }
    }
}

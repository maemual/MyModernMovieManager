using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.DoubanActorInfo.Storage
{
    public class DoubanActorInfo
    {
        /// <summary>
        /// 豆瓣ID
        /// </summary>
        public virtual int id { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public virtual string name { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public virtual string nameEn { get; set; }
        /// <summary>
        /// 影人头像路径
        /// </summary>
        public virtual string avatars { get; set; }
        /// <summary>
        /// 性别，f/m
        /// </summary>
        public virtual string gender { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        public virtual string bornPlace { get; set; }
    }
}

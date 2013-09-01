using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fish.MovieManager.DoubanMovieInfo;

namespace Fish.MovieManager.DoubanControl
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        public Fish.MovieManager.DoubanMovieInfo.Storage.DoubanMovieInfo GetDoubanMovieInfo(string name)
        {
            var movie = Fish.MovieManager.DoubanAPI.Class.Instance.GetMovieInfo(name);
            return movie;
        }
    }
}

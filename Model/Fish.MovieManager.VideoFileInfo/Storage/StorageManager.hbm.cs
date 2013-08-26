using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Fish.MovieManager.VideoFileInfo.Storage
{
    public sealed partial class StorageManager
    {
        #region 需要修改的地方
        private const string CONN_NAME = "moviemanager";

        public HbmMapping BuildMapping()
        {
            var mapper = new ModelMapper();

            mapper.Class<VideoFileInfo>(mapping =>
            {
                mapping.Table("videofileinfo");
                mapping.Lazy(false);

                mapping.Id(model => model.id, map =>
                {
                    map.Column("id");
                    map.Generator(Generators.Identity);
                });

                mapping.Property(model => model.audioBitRate, map => map.Column("audioBitRate"));
                mapping.Property(model => model.audioFormat, map => map.Column("audioFormat"));
                mapping.Property(model => model.bitRate, map => map.Column("bitRate"));
                mapping.Property(model => model.doubanId, map => map.Column("doubanId"));
                mapping.Property(model => model.duration, map => map.Column("duration"));
                mapping.Property(model => model.extension, map => map.Column("extension"));
                mapping.Property(model => model.frameRate, map => map.Column("frameRate"));
                mapping.Property(model => model.height, map => map.Column("height"));
                mapping.Property(model => model.md5, map => map.Column("md5"));
                mapping.Property(model => model.path, map => map.Column("path"));
                mapping.Property(model => model.totalFrames, map => map.Column("totalFrames"));
                mapping.Property(model => model.userRating, map => map.Column("userRating"));
                mapping.Property(model => model.videoBitRate, map => map.Column("videoBitRate"));
                mapping.Property(model => model.videoFormat, map => map.Column("videoFormat"));
                mapping.Property(model => model.width, map => map.Column("width"));
            });
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
        #endregion

        #region 基本不用动
        private ISessionFactory sessionFactory;
        private static readonly StorageManager _instance = new StorageManager();
        public static StorageManager Instance { get { return _instance; } }

        private StorageManager()
        {
            try
            {
                Configuration configuration = new Configuration();
                configuration.AddProperties(BuildConfig());
                configuration.AddMapping(BuildMapping());
                sessionFactory = configuration.BuildSessionFactory();
            }
            catch
            {
                throw;
            }
        }

        private static IDictionary<string, string> BuildConfig()
        {
            IDictionary<string, string> config = new Dictionary<string, string>();
            config["connection.driver_class"] = "NHibernate.Driver.MySqlDataDriver";
            config["connection.connection_string"] = System.Configuration.ConfigurationManager.ConnectionStrings[CONN_NAME].ConnectionString;
            config["dialect"] = "NHibernate.Dialect.MySQLDialect";
            return config;
        }

        public ISession OpenSession()
        {
#if OutputSql
            return sessionFactory.OpenSession(new SqlInterceptor());
#else
            return sessionFactory.OpenSession();
#endif
        }
        #endregion
    }
}

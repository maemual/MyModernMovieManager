using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Fish.MovieManager.DoubanMovieInfo.Storage
{
    public sealed partial class StorageManager
    {
        #region 需要修改的地方
        private const string CONN_NAME = "moviemanager";

        public HbmMapping BuildMapping()
        {
            var mapper = new ModelMapper();

            mapper.Class<DoubanMovieInfo>(mapping =>
            {
                mapping.Table("doubanmovieinfo");
                mapping.Lazy(false);

                mapping.Id(model => model.doubanId, map =>
                {
                    map.Column("doubanId");
                    map.Generator(Generators.Assigned);
                });

                mapping.Property(model => model.title, map => map.Column("title"));
                mapping.Property(model => model.originalTitle, map => map.Column("originalTitle"));
                mapping.Property(model => model.aka, map => map.Column("aka"));
                mapping.Property(model => model.rating, map => map.Column("rating"));
                mapping.Property(model => model.ratingsCount, map => map.Column("ratingsCount"));
                mapping.Property(model => model.directors, map => map.Column("directors"));
                mapping.Property(model => model.doubanSite, map => map.Column("doubanSite"));
                mapping.Property(model => model.image, map => map.Column("images"));
                mapping.Property(model => model.year, map => map.Column("year"));
                mapping.Property(model => model.countries, map => map.Column("countries"));
                mapping.Property(model => model.summary, map => map.Column("summary"));
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

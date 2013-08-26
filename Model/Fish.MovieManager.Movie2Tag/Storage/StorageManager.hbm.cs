using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Fish.MovieManager.Movie2Tag.Storage
{
    public sealed partial class StorageManager
    {
        #region 需要修改的地方
        private const string CONN_NAME = "moviemanager";

        public HbmMapping BuildMapping()
        {
            var mapper = new ModelMapper();

            mapper.Class<Movie2Tag>(mapping =>
            {
                mapping.Table("movie2tag");
                mapping.Lazy(false);

                mapping.Id(model => model.id, map =>
                {
                    map.Column("id");
                    map.Generator(Generators.Assigned);
                });

                mapping.Property(model => model.tag, map => map.Column("tag"));
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

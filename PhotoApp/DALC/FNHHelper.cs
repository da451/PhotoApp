using System;
using System.Configuration;
using System.Linq;
using DALC.Entities;
using DALC.Interfaces;
using DALC.Repository;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using NHibernate.Type;
using Configuration = NHibernate.Cfg.Configuration;

namespace DALC
{
    public static class FNHHelper
    {
        #region Session

        private static readonly object m_syncRoot = new object();

        private static ISession m_session = null;

        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (m_syncRoot)
                    {
                        if (_sessionFactory == null)
                        {
                            string str;
                            try
                            {
                                str = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            }
                            catch
                            {
                                str = @"Data Source=D:\Git\Projects\PhotoApp\PhotoApp\DALC\DB\PhotoDataBase.sdf";
                            }

                            var dbConfig = MsSqlCeConfiguration.Standard
                                .ConnectionString(str)
                                .Driver<SqlClientDriver>()
                                .Dialect<MsSqlCeDialect>()
                                .Driver<SqlServerCeDriver>()
                                .ShowSql();

                            _sessionFactory = Fluently.Configure()
                                .Database(dbConfig)
                                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Image>())
                                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PColor>())
                                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
                                .BuildSessionFactory();

                            _sessionFactory.Evict(typeof(Image));
                            _sessionFactory.Evict(typeof(PColor));
                        }
                    }
                }
                return _sessionFactory;
            }
        }

        private static void BuildSchema(Configuration configuration)
        {
            SchemaExport schemaExport = new SchemaExport(configuration);
            schemaExport.Execute(false, true, false);
        }

        private static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void CreateSession()
        {
            ISession sess = SessionFactory.OpenSession();
            sess.Close();
        }

        public static IUnitOfWork CreateUoW()
        {
            return new UnitOfWork(_sessionFactory);
        }
        #endregion

    }
}

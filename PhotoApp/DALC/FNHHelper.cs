using System;
using DALC.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;

namespace DALC
{
    public static class FNHHelper
    {
        #region Session
        static FNHHelper()
        {
            OpenSession();
        }

        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    string str = @"Data Source=D:\Git\Projects\PhotoApp\PhotoApp\PhotoApp\DB\PhotoDataBase.sdf;";

                    var dbConfig = MsSqlCeConfiguration.Standard
                        .ConnectionString(str)
                        .Driver<SqlClientDriver>()
                        .Dialect<MsSqlCeDialect>()
                        .Driver<SqlServerCeDriver>()
                        .ShowSql();

                    _sessionFactory = Fluently.Configure()
                        .Database(dbConfig)
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Image>())
                        //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ColorPalette>())
                        //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ColorsOrder>())
                        .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
                        .BuildSessionFactory();


                }
                return _sessionFactory;
            }
        }

        private static void BuildSchema(Configuration configuration)
        {
            SchemaExport schemaExport = new SchemaExport(configuration);
            schemaExport.Execute(false, true, false);
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
        #endregion

        #region Generics
        private static List<T> SelectAll<T>() where T : class
        {
            ICriteria criteria = OpenSession().CreateCriteria<T>();
            List<T> res = (List<T>)criteria.List<T>();
            return res;
        }

        private static void Create<T>(T value)
        {
            ISession sess = FNHHelper.OpenSession();
            ITransaction tx = sess.BeginTransaction(); ;
            try
            {

                sess.Save(value);
                tx.Commit();
            }
            catch (Exception e)
            {
                if (tx != null)
                    tx.Rollback();
                throw;
            }
            finally
            {
                sess.Close();
            }

        }
        #endregion

        #region Image
        public static void CreateImage(byte[] img, byte[] thambnail, string name)
        {
            Create<Image>(new Image(img, thambnail, name));
        }

        public static List<Image> SelectAllImages()
        {
            return SelectAll<Image>();
        }

        #endregion
    }
}

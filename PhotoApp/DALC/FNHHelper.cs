using System;
using System.Configuration;
using DALC.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using Configuration = NHibernate.Cfg.Configuration;

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
                    var str = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

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

        private static bool Delete<T>(string columnNameID, int id)
        {
            int rowCount = 0;

            ISession sess = FNHHelper.OpenSession();
            
            ITransaction tx = sess.BeginTransaction(); ;
            
            try
            {
                var queryString = string.Format("DELETE FROM {0} WHERE {1} = :{2}",
                    typeof(T), columnNameID, columnNameID);

                rowCount = sess.CreateQuery(queryString).SetInt32(columnNameID, id).ExecuteUpdate();
            
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

            return rowCount != 0;
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

        public static bool DeleteImage(int imageID)
        {
            return Delete<Image>("ImageID", imageID);
        }
        #endregion

        #region PColor
        public static void CreatePColor(string name, string value)
        {
            Create<PColor>(new PColor(value, name));
        }

        public static List<PColor> SelectAllPColors()
        {
            return SelectAll<PColor>();
        }

        public static bool DeletePColor(int colorID)
        {
            return Delete<PColor>("ColorID", colorID);
        } 
        #endregion
    }
}

using System;
using System.Configuration;
using System.Linq;
using DALC.Entities;
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
        static FNHHelper()
        {
            OpenSession();
        }

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
            SessionFactory.OpenSession();
        }
        #endregion

        #region Generics
        private static List<T> SelectAll<T>() where T : class
        {

            ISession sess = OpenSession();
            try
            {
                ICriteria criteria = sess.CreateCriteria<T>();
                List<T> res = (List<T>)criteria.SetCacheMode(CacheMode.Ignore).SetCacheable(false).List<T>();
                return res;
            }
            finally
            {
                sess.Close();
            }
        }

        private static T SelectByID<T>(string columnNameID, int id) where T : class
        {
            ISession sess = OpenSession();

            try
            {
                ICriteria criteria = sess.CreateCriteria<T>().SetCacheMode(CacheMode.Ignore)
                    .SetCacheable(false).Add(Restrictions.Eq(columnNameID, id));

                T res = (T)criteria.List<T>().First();
                return res;
            }
            finally
            {
                sess.Close();
            }
        }

        private static int Create<T>(T value)
        {
            ISession sess = OpenSession();
            ITransaction tx = sess.BeginTransaction();
            int id=-1;
            try
            {

                id = Convert.ToInt32(sess.Save(value));
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
            return id;
        }

        private static bool Update<T>(T value) where T : class 
        {
            int rowCount = 0;
            ISession sess = OpenSession();

            ITransaction tx = sess.BeginTransaction(); ;

            try
            {
                sess.Merge<T>(value);
                
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

        private static bool Delete<T>(string columnNameID, int id)
        {
            int rowCount = 0;

            ISession sess = OpenSession();
            
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
        public static int CreateImage(byte[] img, byte[] thambnail, string name)
        {
            return Create<Image>(new Image(img, thambnail, name, new List<PColor>()));
        }

        public static int CreateImage(byte[] img, byte[] thambnail, string name, IList<PColor> colors )
        {
            return Create<Image>(new Image(img, thambnail, name,colors));
        }

        public static List<Image> SelectAllImages()
        {
            return SelectAll<Image>();
        }

        public static Image SelectImagesByID(int imageID)
        {
            return SelectByID<Image>("ImageID",imageID);
        }

        public static bool DeleteImage(int imageID)
        {
            return Delete<Image>("ImageID", imageID);
        }

        public static void UpdateImage(Image image)
        {
            Update<Image>(image);
        }
        #endregion

        #region PColor
        public static int CreatePColor(string name, string value)
        {
            return Create<PColor>(new PColor(value, name));
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

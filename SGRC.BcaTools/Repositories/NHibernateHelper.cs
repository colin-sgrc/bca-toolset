using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration().Configure();
                    configuration.AddAssembly(typeof(Sale).Assembly);
                    _sessionFactory = configuration.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactory.OpenSession();
        }

        public static IStatelessSession GetCurrentStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        public static void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
                _sessionFactory.Dispose();
                _sessionFactory = null;

            }
        }
    }
}


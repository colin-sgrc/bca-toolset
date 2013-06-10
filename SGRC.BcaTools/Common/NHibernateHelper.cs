#region License, Terms and Conditions
//
// BCAAImport: BC Assessment Import Utility
// Written by Colin Dyck (Selkirk Geospatial Research Centre)
// Copyright (c) 2012. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License
// along with this library; If not, see <http://www.gnu.org/licenses/>.
//
#endregion

using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;

namespace SGRC.BCATools
{
    /// <summary>
    /// Holds NHibernate session object
    /// </summary>
    public sealed class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <value>
        /// The session factory.
        /// </value>
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = GetCurrentConfiguration().BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        private static Configuration _configuration;
        /// <summary>
        /// Gets the current configuration.
        /// </summary>
        /// <returns></returns>
        public static Configuration GetCurrentConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new Configuration().Configure();
                _configuration.AddAssembly(typeof(Sale).Assembly);
            }

            return _configuration;
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// Gets the current stateless session.
        /// </summary>
        /// <returns></returns>
        public static IStatelessSession GetCurrentStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        /// <summary>
        /// Closes the session factory.
        /// </summary>
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


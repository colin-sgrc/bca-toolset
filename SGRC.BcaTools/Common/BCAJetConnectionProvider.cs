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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Connection;

namespace SGRC.BCATools
{
    /// <summary>
    /// Sets appropriate Jet connection string from global settings
    /// </summary>
    public class BCAJetConnectionProvider : DriverConnectionProvider
    {
        private string _connectionString;

        /// <summary>
        /// Configures the ConnectionProvider with the Driver and the ConnectionString.
        /// </summary>
        /// <param name="settings">An <see cref="T:System.Collections.IDictionary" /> that contains the settings for this ConnectionProvider.</param>
        public override void Configure(IDictionary<string, string> settings)
        {
            //bit of a hack i guess...grab the msaccess file from session Settings
            _connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", BCASession.Current.DestinationDb);
            settings["connection.connection_string"] = _connectionString;

            ConfigureDriver(settings);
        }

        /// <summary>
        /// Gets the <see cref="T:System.String" /> for the <see cref="T:System.Data.IDbConnection" />
        /// to connect to the database.
        /// </summary>
        /// <value>
        /// The <see cref="T:System.String" /> for the <see cref="T:System.Data.IDbConnection" />
        /// to connect to the database.
        /// </value>
        protected override string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
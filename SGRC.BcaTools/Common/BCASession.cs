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
using System.Runtime.InteropServices;
using System.Reflection;

using log4net;
using NHibernate;

namespace SGRC.BCATools
{
    /// <summary>
    /// Common functionality
    /// </summary>
    public class BCASession
    {
        public string ImportType { get; set; }
        public string SourceDatFile { get; set; }
        public string DestinationDb { get; set; }
        private ILog _log = null;

        private static BCASession _current;
        public static BCASession Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new BCASession();
                }

                return _current;
            }
        }

        public ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(BCAObjectLoader));
                }
                return _log;
            }
        }

        /// <summary>
        /// Configures the log.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public void ConfigureLog(string logName)
        {
            if (_log != null)
            {
                LogManager.Shutdown();
                _log = null;
            }

            log4net.GlobalContext.Properties["LogName"] = logName;
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Parses the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static T ParseRecord<T>(string line)
        {
            IntPtr intPtr = Marshal.StringToBSTR(line);
            T structure = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeBSTR(intPtr);

            return (T)structure;
        }

    }
}

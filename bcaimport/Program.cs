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

using NDesk.Options;

namespace SGRC.BCATools
{
    /// <summary>
    /// Command line utility for the BCA Data Advice Toolset Utility
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool showHelp = false;

                var parser = new OptionSet() 
                {
                    { "h|?|help",  "Show available options", v => showHelp = true },
                    { "i=|import_type=", "dat file format (year, month)", v => BCASession.Current.ImportType = v },
                    { "s=|source=", "source dat file", v => BCASession.Current.SourceDatFile = v },
                    { "d=|destination", "destination database (MS Access)", v => BCASession.Current.DestinationDb = v }
                };
                parser.Parse(args);

                if (showHelp)
                {
                    parser.WriteOptionDescriptions(Console.Out);
                    return;
                }

                if (string.IsNullOrEmpty(BCASession.Current.ImportType) || !new List<string>() { "year", "month" }.Any(a => a.Equals(BCASession.Current.ImportType, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new ArgumentException("i=|import_type value must be either 'year' or 'month'");
                }

                if (string.IsNullOrEmpty(BCASession.Current.SourceDatFile) || !System.IO.File.Exists(BCASession.Current.SourceDatFile))
                {
                    throw new ArgumentException("Invalid Source Dat File");
                }

                if (string.IsNullOrEmpty(BCASession.Current.DestinationDb) || !System.IO.File.Exists(BCASession.Current.DestinationDb))
                {
                    throw new ArgumentException("Invalid Destination Db file");
                }

                //log name contains import dat name and date
                BCASession.Current.ConfigureLog(string.Format("{0}.{1:dd.MM.yyy hh.mm.ss}", System.IO.Path.GetFileNameWithoutExtension(BCASession.Current.SourceDatFile), DateTime.Now));

                //create the iterator to build all the business objects
                BCAObjectLoader loader = new BCAObjectLoader(BCASession.Current.SourceDatFile);
                loader.Start();
                BCAPersister persist;

                switch (BCASession.Current.ImportType.ToLower())
                {
                    case "year":
                        persist = new BCAPersister();
                        persist.PersistYearlyDatFile(loader);
                        break;

                    case "month":
                        persist = new BCAPersister();
                        persist.PersistYearlyDatFile(loader);
                        break;
                }
            }
            catch (Exception e)
            {
                BCASession.Current.Log.Error(e.Message);
                BCASession.Current.Log.Error(e.StackTrace);
            }
            finally
            {
                BCASession.Current.Log.Info("process complete");
            }
        }
    }
}

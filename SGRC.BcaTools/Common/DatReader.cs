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

using MiscUtil.IO;
using log4net;
using log4net.Config;

namespace SGRC.BCATools
{
    /// <summary>
    /// Args for iterator
    /// </summary>
    public class DatIteratorEventArgs : EventArgs
    {
        public int LineIndex { get; set; }
        public string Line { get; set; }
    }

    /// <summary>
    /// Responsible for reading a BCAA DAT file line by line
    /// </summary>
    public abstract class DatReader
    {
        protected string DatFile;
        protected static ILog log = LogManager.GetLogger(typeof(DatReader));

        /// <summary>
        /// Initializes a new instance of the <see cref="DatReader" /> class.
        /// </summary>
        /// <param name="datFile">The dat file.</param>
        protected DatReader(string datFile)
        {
            DatFile = datFile;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            LineReader lineReader = new LineReader(DatFile);
            IEnumerator<string> lines = lineReader.GetEnumerator();
            int lineIndex = 0;

            //get all rows after header
            while (lines.MoveNext())
            {
                OnLineMoveNext(this, new DatIteratorEventArgs() { LineIndex = lineIndex, Line = lines.Current });
                lineIndex++;
            }
            Cleanup();
        }

        /// <summary>
        /// Called when [line move next].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        protected virtual void OnLineMoveNext(object sender, DatIteratorEventArgs args) { }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        protected virtual void Cleanup() { }

    }
}

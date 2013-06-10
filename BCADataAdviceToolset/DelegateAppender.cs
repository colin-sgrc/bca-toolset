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

using log4net.Core;

//idea from http://www.nimblecoder.com/blog/archive/2009/01/30/using-a-delegate-and-custom-appender-with-log4net-to-display.aspx
namespace SGRC.BCATools
{
    public delegate void OnEventLogged(LoggingEvent loggingEvent);

    /// <summary>
    /// delegates log4net messages to listener
    /// </summary>
    public class DelegateAppender : log4net.Appender.AppenderSkeleton
    {
        /// <summary>
        /// Gets or sets the on event logged.
        /// </summary>
        /// <value>
        /// The on event logged.
        /// </value>
        public OnEventLogged OnEventLogged { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAppender" /> class.
        /// </summary>
        public DelegateAppender() {}

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton" /> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        ///   <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent" />.
        ///   </para>
        ///   <para>This method will be called by <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" />
        /// if all the conditions listed for that method are met.
        ///   </para>
        ///   <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:log4net.Appender.AppenderSkeleton.PreAppendCheck" /> method.
        ///   </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (OnEventLogged != null)
            {
                OnEventLogged(loggingEvent);
            }
        }

    }
}

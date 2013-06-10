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

namespace SGRC.BCATools
{
    /// <summary>
    /// AdditionalOwner domain object
    /// </summary>
    public class AdditionalOwner : IDomainObject
    {
        public virtual string FolioGroup { get; set; }
        public virtual string Folio { get; set; }
        public virtual string GroupCode { get; set; }
        public virtual int Jurisdiction { get; set; }
        public virtual int Roll { get; set; }
        public virtual int? Action { get; set; }

        public virtual string Surname1 { get; set; }
        public virtual string FirstName1 { get; set; }
        public virtual string Initial1 { get; set; }
        public virtual string Surname2 { get; set; }
        public virtual string FirstName2 { get; set; }
        public virtual string Initial2 { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Address4 { get; set; }
        public virtual string Address5 { get; set; }
        public virtual string Address6 { get; set; }
    }
}

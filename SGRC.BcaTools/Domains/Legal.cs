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
    /// Legal domain object
    /// </summary>
    public class Legal : IDomainObject
    {
        public virtual string Folio { get; set; }
        public virtual int Jurisdiction { get; set; }
        public virtual int Roll { get; set; }
        public virtual int? Action { get; set; }

        public virtual int? Area { get; set; }
        public virtual string Lot { get; set; }
        public virtual string Block { get; set; }
        public virtual string Section { get; set; }
        public virtual string Township { get; set; }
        public virtual string PlanNumber { get; set; }
        public virtual string DistrictLot { get; set; }
        public virtual string LandDistrict { get; set; }
        public virtual int LotSizeCode { get; set; }
        public virtual string AreaUnit { get; set; }
        public virtual double Width { get; set; }
        public virtual double Depth { get; set; }
        public virtual string LotArea { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string AptNumber { get; set; }
        public virtual string StreetDirection { get; set; }
        public virtual string StreetName { get; set; }
        public virtual string PID { get; set; }
        public virtual string Description1 { get; set; }
        public virtual string Description2 { get; set; }
        public virtual string Description3 { get; set; }
        public virtual string Description4 { get; set; }
        public virtual string Description5 { get; set; }
        public virtual string Description6 { get; set; }
        public virtual string Description7 { get; set; }
        public virtual string Description8 { get; set; }
        public virtual string Description9 { get; set; }
    }
}

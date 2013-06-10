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
    /// Assessment domain object
    /// </summary>
    public class Assessment : IDomainObject
    {
        public virtual string Folio { get; set; }
        public virtual int Jurisdiction { get; set; }
        public virtual int Roll { get; set; }
        public virtual int? Action { get; set; }

        public virtual string SchoolDistrict { get; set; }
        public virtual string ElectoralArea { get; set; }
        public virtual string ImprovementDistrict { get; set; }
        public virtual string LocalArea { get; set; }
        public virtual string SpecifiedArea { get; set; }
        
        public virtual int LandUseCode { get; set; }
        public virtual string ActualUse { get; set; }
        public virtual string RollYear { get; set; }

        public virtual int ExemptCode1 { get; set; }
        public virtual int LandClass1 { get; set; }
        public virtual double LandValue1 { get; set; }
        public virtual int ImprovementClass1 { get; set; }
        public virtual double ImprovementValue1 { get; set; }

        public virtual int ExemptCode2 { get; set; }
        public virtual int LandClass2 { get; set; }
        public virtual double LandValue2 { get; set; }
        public virtual int ImprovementClass2 { get; set; }
        public virtual double ImprovementValue2 { get; set; }

        public virtual int ExemptCode3 { get; set; }
        public virtual int LandClass3 { get; set; }
        public virtual double LandValue3 { get; set; }
        public virtual int ImprovementClass3 { get; set; }
        public virtual double? ImprovementValue3 { get; set; }

        public virtual double ExtendedLand1 { get; set; }
        public virtual double ExtendedImprovement1 { get; set; }
        public virtual double ExtendedLand2 { get; set; }
        public virtual double ExtendedImprovement2 { get; set; }
        public virtual double ExtendedLand3 { get; set; }
        public virtual double ExtendedImprovement3 { get; set; }
    }
}

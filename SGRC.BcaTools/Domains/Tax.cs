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
    /// Tax domain object
    /// </summary>
    public class Tax : IDomainObject
    {
        public virtual string Folio { get; set; }
        public virtual int Jurisdiction { get; set; }
        public virtual int Roll { get; set; }
        public virtual int? Action { get; set; }

        public virtual int MunicipalClassCode1 { get; set; }
        public virtual string MunicipalClassDescription1 { get; set; }
        public virtual double MunicipalGrossLand1 { get; set; }
        public virtual double MunicipalGrossImprovement1 { get; set; }
        public virtual double MunicipalExemptLand1 { get; set; }
        public virtual double MunicipalExemptImprovement1 { get; set; }

        public virtual int MunicipalClassCode2 { get; set; }
        public virtual string MunicipalClassDescription2 { get; set; }
        public virtual double MunicipalGrossLand2 { get; set; }
        public virtual double MunicipalGrossImprovement2 { get; set; }
        public virtual double MunicipalExemptLand2 { get; set; }
        public virtual double MunicipalExemptImprovement2 { get; set; }

        public virtual int MunicipalClassCode3 { get; set; }
        public virtual string MunicipalClassDescription3 { get; set; }
        public virtual double MunicipalGrossLand3 { get; set; }
        public virtual double MunicipalGrossImprovement3 { get; set; }
        public virtual double MunicipalExemptLand3 { get; set; }
        public virtual double MunicipalExemptImprovement3 { get; set; }

        public virtual int MunicipalClassCode4 { get; set; }
        public virtual string MunicipalClassDescription4 { get; set; }
        public virtual double MunicipalGrossLand4 { get; set; }
        public virtual double MunicipalGrossImprovement4 { get; set; }
        public virtual double MunicipalExemptLand4 { get; set; }
        public virtual double MunicipalExemptImprovement4 { get; set; }

        public virtual int SchoolClassCode1 { get; set; }
        public virtual string SchoolClassDescription1 { get; set; }
        public virtual double SchoolGrossLand1 { get; set; }
        public virtual double SchoolGrossImprovement1 { get; set; }
        public virtual double SchoolExemptLand1 { get; set; }
        public virtual double SchoolExemptImprovement1 { get; set; }

        public virtual int SchoolClassCode2 { get; set; }
        public virtual string SchoolClassDescription2 { get; set; }
        public virtual double SchoolGrossLand2 { get; set; }
        public virtual double SchoolGrossImprovement2 { get; set; }
        public virtual double SchoolExemptLand2 { get; set; }
        public virtual double SchoolExemptImprovement2 { get; set; }

        public virtual int SchoolClassCode3 { get; set; }
        public virtual string SchoolClassDescription3 { get; set; }
        public virtual double SchoolGrossLand3 { get; set; }
        public virtual double SchoolGrossImprovement3 { get; set; }
        public virtual double SchoolExemptLand3 { get; set; }
        public virtual double SchoolExemptImprovement3 { get; set; }

        public virtual int SchoolClassCode4 { get; set; }
        public virtual string SchoolClassDescription4 { get; set; }
        public virtual double SchoolGrossLand4 { get; set; }
        public virtual double SchoolGrossImprovement4 { get; set; }
        public virtual double SchoolExemptLand4 { get; set; }
        public virtual double SchoolExemptImprovement4 { get; set; }
    }
}

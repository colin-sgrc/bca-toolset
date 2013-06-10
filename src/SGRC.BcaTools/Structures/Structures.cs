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

//file contains all fixed string structure definitions that map to a BCAA Record Layout
//see BC Assessment Data Advice User Guide Section 8
//not all are defined...only did ones immediately needed for project
namespace SGRC.BCATools
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct BCAHeaderRecord
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct BCARecord
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct SalesData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Certificate1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Date1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string Price1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Type1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Certificate2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Date2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string Price2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Type2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Certificate3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Date3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string Price3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Type3;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct FixedLegalDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Lot;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Block;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Section;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Township;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Range;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Meridian;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string PlanNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string DistrictLot;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandDistrict;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string LotSizeKey;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string LotSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string StreetNumberFrom;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string StreetNumberToOrApt;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string StreetDirection;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string StreetName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string NTSNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Unused2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string PropertyIdentificationNumber;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct FreeFormLegalDescription
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string FreeForm1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string FreeForm2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MHR;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct Valuation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string SchoolDistrict;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string RegionalDistrict;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ElectoralArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string RegionalHospitalDistrict;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImprovementDistrict;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LocalArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string SpecifiedDefined;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string IndianBand;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Internal;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string ManualClassCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ManualClassDeviation;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string FarmUnit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string NeighborhoodCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandUseCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string ActualUse;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Unused3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string RollYear;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue3;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct ExtendedValuation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ExemptCode3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LandClass3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LandValue3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ImproveClass3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ImproveValue3;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct Ownership
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 23)]
        public string Unused2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string Surname1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string FirstName1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Initial1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string Surname2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string FirstName2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string Initial2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string PostalCode;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct OwnershipAddress
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Address1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Address2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Address3;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct TaxableValues
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AssessmentArea;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Jurisdiction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AssessmentRollNumberUnused;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AssessmentRollNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string Unused1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string GroupCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string RecordNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string ActionCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ClassCode1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossLand1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossImprovement1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptLand1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptImprovement1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ClassCode2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossLand2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossImprovement2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptLand2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptImprovement2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ClassCode3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossLand3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossImprovement3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptLand3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptImprovement3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string ClassCode4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossLand4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GrossImprovement4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptLand4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExemptImprovement4;
    }

}

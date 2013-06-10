using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDKB.BCAAImport
{
    static class DomainObjectStructMapper
    {
        public static void FillSale(Sale sale, SalesData saleStructure)
        {
            sale.Folio = saleStructure.Jurisdiction + saleStructure.AssessmentRollNumber;
            sale.Jurisdiction = NullableParser.TryParseInt(saleStructure.Jurisdiction);
            sale.Roll = NullableParser.TryParseInt(saleStructure.AssessmentRollNumber);
            sale.Action = NullableParser.TryParseInt(saleStructure.ActionCode).GetValueOrDefault(0);

            sale.Certificate1 = NullableParser.ParseString(saleStructure.Certificate1);
            sale.Type1 = NullableParser.ParseString(saleStructure.Type1);
            sale.Date1 = ParseSaleDate(saleStructure.Date1);
            sale.Price1 = NullableParser.TryParseDouble(saleStructure.Price1).GetValueOrDefault(0);

            sale.Certificate2 = NullableParser.ParseString(saleStructure.Certificate2);
            sale.Type2 = NullableParser.ParseString(saleStructure.Type2);
            sale.Date2 = ParseSaleDate(saleStructure.Date2);
            sale.Price2 = NullableParser.TryParseDouble(saleStructure.Price2).GetValueOrDefault(0);

            sale.Certificate3 = NullableParser.ParseString(saleStructure.Certificate3);
            sale.Type3 = NullableParser.ParseString(saleStructure.Type3);
            sale.Date3 = ParseSaleDate(saleStructure.Date3);
            sale.Price3 = NullableParser.TryParseDouble(saleStructure.Price3).GetValueOrDefault(0);
            sale.Updated = "N";
        }

        public static void FillLegal(Legal legal, FixedLegalDescription legalStruct)
        {
            legal.Folio = legalStruct.Jurisdiction + legalStruct.AssessmentRollNumber;
            legal.Jurisdiction = NullableParser.TryParseInt(legalStruct.Jurisdiction);
            legal.Roll = NullableParser.TryParseInt(legalStruct.AssessmentRollNumber);
            legal.Action = NullableParser.TryParseInt(legalStruct.ActionCode).GetValueOrDefault(0);

            legal.Area = NullableParser.TryParseInt(legalStruct.AssessmentArea);
            legal.Lot = NullableParser.ParseString(legalStruct.Lot);
            legal.Block = NullableParser.ParseString(legalStruct.Block);
            legal.Section = NullableParser.ParseString(legalStruct.Section);
            legal.Township = NullableParser.ParseString(legalStruct.Township);
            legal.PlanNumber = NullableParser.ParseString(legalStruct.PlanNumber);
            legal.DistrictLot = NullableParser.ParseString(legalStruct.DistrictLot);
            legal.LandDistrict = NullableParser.ParseString(legalStruct.LandDistrict);
            legal.StreetNumber = NullableParser.ParseString(legalStruct.StreetNumberFrom);
            legal.AptNumber = NullableParser.ParseString(legalStruct.StreetNumberToOrApt);
            legal.StreetName = NullableParser.ParseString(legalStruct.StreetName);
            legal.PID = NullableParser.ParseString(legalStruct.PropertyIdentificationNumber);

            legal.LotSizeCode = NullableParser.TryParseInt(legalStruct.LotSizeKey).GetValueOrDefault(0);

            switch (legal.LotSizeCode)
            {
                case 0:
                    legal.AreaUnit = "wxd";
                    legal.Width = Math.Round(NullableParser.TryParseDouble(legalStruct.LotSize.Substring(0, 7)).GetValueOrDefault(0), MidpointRounding.ToEven);
                    legal.Depth = Math.Round(NullableParser.TryParseDouble(legalStruct.LotSize.Substring(7, 7)).GetValueOrDefault(0), MidpointRounding.ToEven);
                    break;

                case 1:
                    legal.AreaUnit = "dim";
                    legal.Width = Math.Round(NullableParser.TryParseDouble(legalStruct.LotSize.Substring(0, 7)).GetValueOrDefault(0), MidpointRounding.ToEven);
                    legal.Depth = Math.Round(NullableParser.TryParseDouble(legalStruct.LotSize.Substring(7, 7)).GetValueOrDefault(0), MidpointRounding.ToEven);
                    break;

                case 2:
                    legal.AreaUnit = "sqf";
                    legal.LotArea = NullableParser.ParseString(legalStruct.LotSize);
                    break;

                case 3:
                    legal.AreaUnit = "acr";
                    legal.LotArea = NullableParser.ParseString(legalStruct.LotSize);
                    break;

                default:
                    legal.AreaUnit = "ff";
                    legal.LotArea = NullableParser.ParseString(legalStruct.LotSize);
                    break;
            }
            legal.Updated = "N";
        }

        public static void FillLegal(Legal legal, FreeFormLegalDescription legalStruct)
        {
            string recordCode = legalStruct.GroupCode + legalStruct.RecordNo;
            switch (recordCode)
            {
                case BCAACommon.RecordCode_LegalFreeform1:
                    legal.Description1 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description2 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description3 = NullableParser.ParseString(legalStruct.MHR);
                    break;

                case BCAACommon.RecordCode_LegalFreeform2:
                    legal.Description4 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description5 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description6 = NullableParser.ParseString(legalStruct.MHR);
                    break;

                case BCAACommon.RecordCode_LegalFreeform3:
                    legal.Description7 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description8 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description9 = NullableParser.ParseString(legalStruct.MHR);
                    break;
            }
        }

        public static void FillOwner(Owner owner, string line)
        {
            BCAARecord record = BCAACommon.ParseRecord<BCAARecord>(line);

            switch (record.RecordNo)
            {
                case "1":
                    Ownership ownerStruct = BCAACommon.ParseRecord<Ownership>(line);
                    owner.Folio = ownerStruct.Jurisdiction + ownerStruct.AssessmentRollNumber;
                    owner.Jurisdiction = NullableParser.TryParseInt(ownerStruct.Jurisdiction);
                    owner.Roll = NullableParser.TryParseInt(ownerStruct.AssessmentRollNumber);
                    owner.Action = NullableParser.TryParseInt(ownerStruct.ActionCode).GetValueOrDefault(0);

                    owner.Surname1 = NullableParser.ParseString(ownerStruct.Surname1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName1 = NullableParser.ParseString(ownerStruct.FirstName1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial1 = NullableParser.ParseString(ownerStruct.Initial1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Surname2 = NullableParser.ParseString(ownerStruct.Surname2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName2 = NullableParser.ParseString(ownerStruct.FirstName2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial2 = NullableParser.ParseString(ownerStruct.Initial2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.PostalCode = NullableParser.ParseString(ownerStruct.PostalCode);
                    owner.MoreOwners = "No";
                    owner.Updated = "N";
                    break;

                case "2":
                    OwnershipAddress ownerExtendedStruct2 = BCAACommon.ParseRecord<OwnershipAddress>(line);
                    owner.Address1 = NullableParser.ParseString(ownerExtendedStruct2.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address2 = NullableParser.ParseString(ownerExtendedStruct2.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address3 = NullableParser.ParseString(ownerExtendedStruct2.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;

                case "3":
                    OwnershipAddress ownerExtendedStruct3 = BCAACommon.ParseRecord<OwnershipAddress>(line);
                    owner.Address4 = NullableParser.ParseString(ownerExtendedStruct3.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address5 = NullableParser.ParseString(ownerExtendedStruct3.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address6 = NullableParser.ParseString(ownerExtendedStruct3.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;
            }
        }

        public static void FillAdditionalOwner(AdditionalOwner owner, string line)
        {
            BCAARecord record = BCAACommon.ParseRecord<BCAARecord>(line);

            switch (record.RecordNo)
            {
                case "1":
                    Ownership ownerStruct = BCAACommon.ParseRecord<Ownership>(line);
                    owner.Folio = ownerStruct.Jurisdiction + ownerStruct.AssessmentRollNumber;
                    owner.GroupCode = NullableParser.ParseString(ownerStruct.GroupCode);
                    owner.FolioGroup = owner.Folio + owner.GroupCode;

                    owner.Jurisdiction = NullableParser.TryParseInt(ownerStruct.Jurisdiction);
                    owner.Roll = NullableParser.TryParseInt(ownerStruct.AssessmentRollNumber);
                    owner.Action = NullableParser.TryParseInt(ownerStruct.ActionCode).GetValueOrDefault(0);

                    owner.Surname1 = NullableParser.ParseString(ownerStruct.Surname1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName1 = NullableParser.ParseString(ownerStruct.FirstName1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial1 = NullableParser.ParseString(ownerStruct.Initial1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Surname2 = NullableParser.ParseString(ownerStruct.Surname2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName2 = NullableParser.ParseString(ownerStruct.FirstName2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial2 = NullableParser.ParseString(ownerStruct.Initial2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.PostalCode = NullableParser.ParseString(ownerStruct.PostalCode);
                    owner.Updated = "N";
                    break;

                case "2":
                    OwnershipAddress ownerExtendedStruct2 = BCAACommon.ParseRecord<OwnershipAddress>(line);
                    owner.Address1 = NullableParser.ParseString(ownerExtendedStruct2.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address2 = NullableParser.ParseString(ownerExtendedStruct2.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address3 = NullableParser.ParseString(ownerExtendedStruct2.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;

                case "3":
                    OwnershipAddress ownerExtendedStruct3 = BCAACommon.ParseRecord<OwnershipAddress>(line);
                    owner.Address4 = NullableParser.ParseString(ownerExtendedStruct3.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address5 = NullableParser.ParseString(ownerExtendedStruct3.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address6 = NullableParser.ParseString(ownerExtendedStruct3.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;
            }
        }

        public static void FillAssessment(Assessment assessment, Valuation valuation)
        {
            assessment.Folio = valuation.Jurisdiction + valuation.AssessmentRollNumber;
            assessment.Jurisdiction = NullableParser.TryParseInt(valuation.Jurisdiction);
            assessment.Roll = NullableParser.TryParseInt(valuation.AssessmentRollNumber);
            assessment.Action = NullableParser.TryParseInt(valuation.ActionCode).GetValueOrDefault(0);

            assessment.SchoolDistrict = NullableParser.ParseString(valuation.SchoolDistrict);
            assessment.ElectoralArea = NullableParser.ParseString(valuation.ElectoralArea);
            assessment.ImprovementDistrict = NullableParser.ParseString(valuation.ImprovementDistrict);
            assessment.LocalArea = NullableParser.ParseString(valuation.LocalArea);
            assessment.SpecifiedArea = NullableParser.ParseString(valuation.SpecifiedDefined);
            assessment.LandUseCode = NullableParser.TryParseInt(valuation.LandUseCode).GetValueOrDefault(0);
            assessment.ActualUse = NullableParser.ParseString(valuation.ActualUse);
            assessment.RollYear = NullableParser.ParseString(valuation.RollYear);

            assessment.ExemptCode1 = NullableParser.TryParseInt(valuation.ExemptCode1).GetValueOrDefault(0);
            assessment.LandClass1 = NullableParser.TryParseInt(valuation.LandClass1).GetValueOrDefault(0);
            assessment.LandValue1 = NullableParser.TryParseDouble(valuation.LandValue1).GetValueOrDefault(0);
            assessment.ImprovementClass1 = NullableParser.TryParseInt(valuation.ImproveClass1).GetValueOrDefault(0);
            assessment.ImprovementValue1 = NullableParser.TryParseDouble(valuation.ImproveValue1).GetValueOrDefault(0);

            assessment.ExemptCode2 = NullableParser.TryParseInt(valuation.ExemptCode2).GetValueOrDefault(0);
            assessment.LandClass2 = NullableParser.TryParseInt(valuation.LandClass2).GetValueOrDefault(0);
            assessment.LandValue2 = NullableParser.TryParseDouble(valuation.LandValue2).GetValueOrDefault(0);
            assessment.ImprovementClass2 = NullableParser.TryParseInt(valuation.ImproveClass2).GetValueOrDefault(0);
            assessment.ImprovementValue2 = NullableParser.TryParseDouble(valuation.ImproveValue2).GetValueOrDefault(0);

            assessment.ExemptCode3 = NullableParser.TryParseInt(valuation.ExemptCode3).GetValueOrDefault(0);
            assessment.LandClass3 = NullableParser.TryParseInt(valuation.LandClass3).GetValueOrDefault(0);
            assessment.LandValue3 = NullableParser.TryParseDouble(valuation.LandValue3).GetValueOrDefault(0);
            assessment.ImprovementClass3 = NullableParser.TryParseInt(valuation.ImproveClass3).GetValueOrDefault(0);
            assessment.ImprovementValue3 = NullableParser.TryParseDouble(valuation.ImproveValue3).GetValueOrDefault(0);

            assessment.Updated = "N";
        }

        public static void FillAssessment(Assessment assessment, ExtendedValuation valuation)
        {
            assessment.ExtendedLand1 = NullableParser.TryParseDouble(valuation.LandValue1).GetValueOrDefault(0);
            assessment.ExtendedImprovement1 = NullableParser.TryParseDouble(valuation.ImproveValue1).GetValueOrDefault(0);

            assessment.ExtendedLand2 = NullableParser.TryParseDouble(valuation.LandValue2).GetValueOrDefault(0);
            assessment.ExtendedImprovement2 = NullableParser.TryParseDouble(valuation.ImproveValue2).GetValueOrDefault(0);

            assessment.ExtendedLand3 = NullableParser.TryParseDouble(valuation.LandValue3).GetValueOrDefault(0);
            assessment.ExtendedImprovement3 = NullableParser.TryParseDouble(valuation.ImproveValue3).GetValueOrDefault(0);
        }

        public static void FillTax(Tax tax, TaxableValues taxableValues)
        {
            tax.Folio = taxableValues.Jurisdiction + taxableValues.AssessmentRollNumber;
            tax.Jurisdiction = NullableParser.TryParseInt(taxableValues.Jurisdiction);
            tax.Roll = NullableParser.TryParseInt(taxableValues.AssessmentRollNumber);
            tax.Action = NullableParser.TryParseInt(taxableValues.ActionCode).GetValueOrDefault(0);

            tax.Updated = "N";

            string recordCode = taxableValues.GroupCode + taxableValues.RecordNo;
            switch (recordCode)
            {
                case BCAACommon.RecordCode_MunicipalTax:
                    tax.MunicipalClassCode1 = NullableParser.TryParseInt(taxableValues.ClassCode1).GetValueOrDefault(0);
                    tax.MunicipalGrossLand1 = NullableParser.TryParseDouble(taxableValues.GrossLand1).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement1 = NullableParser.TryParseDouble(taxableValues.GrossImprovement1).GetValueOrDefault(0);
                    tax.MunicipalExemptLand1 = NullableParser.TryParseDouble(taxableValues.ExemptLand1).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement1 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement1).GetValueOrDefault(0);
                    tax.MunicipalClassCode2 = NullableParser.TryParseInt(taxableValues.ClassCode2).GetValueOrDefault(0);
                    tax.MunicipalGrossLand2 = NullableParser.TryParseDouble(taxableValues.GrossLand2).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement2 = NullableParser.TryParseDouble(taxableValues.GrossImprovement2).GetValueOrDefault(0);
                    tax.MunicipalExemptLand2 = NullableParser.TryParseDouble(taxableValues.ExemptLand2).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement2 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement2).GetValueOrDefault(0);
                    tax.MunicipalClassCode3 = NullableParser.TryParseInt(taxableValues.ClassCode3).GetValueOrDefault(0);
                    tax.MunicipalGrossLand3 = NullableParser.TryParseDouble(taxableValues.GrossLand3).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement3 = NullableParser.TryParseDouble(taxableValues.GrossImprovement3).GetValueOrDefault(0);
                    tax.MunicipalExemptLand3 = NullableParser.TryParseDouble(taxableValues.ExemptLand3).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement3 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement3).GetValueOrDefault(0);
                    tax.MunicipalClassCode4 = NullableParser.TryParseInt(taxableValues.ClassCode4).GetValueOrDefault(0);
                    tax.MunicipalGrossLand4 = NullableParser.TryParseDouble(taxableValues.GrossLand4).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement4 = NullableParser.TryParseDouble(taxableValues.GrossImprovement4).GetValueOrDefault(0);
                    tax.MunicipalExemptLand4 = NullableParser.TryParseDouble(taxableValues.ExemptLand4).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement4 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement4).GetValueOrDefault(0);
                    break;

                case BCAACommon.RecordCode_SchoolTax:
                    tax.SchoolClassCode1 = NullableParser.TryParseInt(taxableValues.ClassCode1).GetValueOrDefault(0);
                    tax.SchoolGrossLand1 = NullableParser.TryParseDouble(taxableValues.GrossLand1).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement1 = NullableParser.TryParseDouble(taxableValues.GrossImprovement1).GetValueOrDefault(0);
                    tax.SchoolExemptLand1 = NullableParser.TryParseDouble(taxableValues.ExemptLand1).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement1 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement1).GetValueOrDefault(0);
                    tax.SchoolClassCode2 = NullableParser.TryParseInt(taxableValues.ClassCode2).GetValueOrDefault(0);
                    tax.SchoolGrossLand2 = NullableParser.TryParseDouble(taxableValues.GrossLand2).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement2 = NullableParser.TryParseDouble(taxableValues.GrossImprovement2).GetValueOrDefault(0);
                    tax.SchoolExemptLand2 = NullableParser.TryParseDouble(taxableValues.ExemptLand2).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement2 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement2).GetValueOrDefault(0);
                    tax.SchoolClassCode3 = NullableParser.TryParseInt(taxableValues.ClassCode3).GetValueOrDefault(0);
                    tax.SchoolGrossLand3 = NullableParser.TryParseDouble(taxableValues.GrossLand3).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement3 = NullableParser.TryParseDouble(taxableValues.GrossImprovement3).GetValueOrDefault(0);
                    tax.SchoolExemptLand3 = NullableParser.TryParseDouble(taxableValues.ExemptLand3).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement3 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement3).GetValueOrDefault(0);
                    tax.SchoolClassCode4 = NullableParser.TryParseInt(taxableValues.ClassCode4).GetValueOrDefault(0);
                    tax.SchoolGrossLand4 = NullableParser.TryParseDouble(taxableValues.GrossLand4).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement4 = NullableParser.TryParseDouble(taxableValues.GrossImprovement4).GetValueOrDefault(0);
                    tax.SchoolExemptLand4 = NullableParser.TryParseDouble(taxableValues.ExemptLand4).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement4 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement4).GetValueOrDefault(0);
                    break;
            }

        }

        private static DateTime? ParseSaleDate(string date)
        {
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);

            DateTime yearResult;
            if (!DateTime.TryParseExact(year, "yyyy", System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out yearResult))
            {
                return null;
            }

            if (month == "00") month = "01";
            DateTime monthResult;
            if (!DateTime.TryParseExact(month, "mm", System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out monthResult))
            {
                return null;
            }

            DateTime retVal = DateTime.ParseExact(string.Format("01/{0}/{1}", month, year), "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.CurrentInfo);
            return retVal;
        }

    }
}

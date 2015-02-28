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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

namespace SGRC.BCATools
{
    /// <summary>
    /// Responsible for reading Assessment DAT line and reading into domain objects
    /// </summary>
    public class BCAObjectLoader : DatReader, IBCAObjectContainer
    {
        //resulting collections
        public Collection<Sale> SalesList { get; private set; }
        public Collection<Legal> LegalList { get; private set; }
        public Collection<Assessment> AssessmentList { get; private set; }
        public Collection<Owner> OwnerList { get; private set; }
        public Collection<AdditionalOwner> AdditionalOwnerList { get; private set; }
        public Collection<Tax> TaxList { get; private set; }

        //temp business objects to house info
        private Sale _sale = null;
        private Assessment _assessment = null;
        private Legal _legal = null;
        private Owner _owner = null;
        private AdditionalOwner _additionalOwner = null;
        private Tax _tax = null;

        private int? _previousRoll;

        //track records per folio
        private List<string> _ownerRecordNos;
        private List<string> _taxRecords;

        //quick lookup list
        private List<string> _folios;

        //tax class code mapping
        //TODO: probably split this out to file
        Dictionary<int, string> _taxClassMap = new Dictionary<int, string>()
            {
                {0, null},
                {1, "Residential"},
                {2, "Utilities"},
                {3, "Supportive Housing"},
                {4, "Major Industry"},
                {5, "Light Industry"},
                {6, "Business & Other"},
                {7, "Managed Forest Land"},
                {8, "Recreation/Non Profit"},
                {9, "Farm"}
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="BCAObjectLoader" /> class.
        /// </summary>
        /// <param name="datFile">The dat file.</param>
        public BCAObjectLoader(string datFile)
            : base(datFile)
        {
            SalesList = new Collection<Sale>();
            LegalList = new Collection<Legal>();
            AssessmentList = new Collection<Assessment>();
            OwnerList = new Collection<Owner>();
            AdditionalOwnerList = new Collection<AdditionalOwner>();
            TaxList = new Collection<Tax>();

            _ownerRecordNos = new List<string>();
            _taxRecords = new List<string>();

            _folios = new List<string>();
        }

        /// <summary>
        /// Called when [line move next].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        /// <exception cref="System.Exception"></exception>
        protected override void OnLineMoveNext(object sender, DatIteratorEventArgs args)
        {
            int lineIndex = args.LineIndex;
            string line = args.Line;

            //this is the header row...process and leave
            if (lineIndex == 0)
            {
                BCAHeaderRecord header = BCASession.ParseRecord<BCAHeaderRecord>(line);
                return;
            }

            BCARecord record = BCASession.ParseRecord<BCARecord>(line);

            //every row maps to a record, this will help determine what type of row it is
            string folio = record.Jurisdiction + record.AssessmentRollNumber;

            //first init for the previous roll...just called once
            if (!_previousRoll.HasValue)
            {
                _previousRoll = int.Parse(record.AssessmentRollNumber);
            }

            //once the roll number changes, add any records then null out, start again
            int currentRoll = int.Parse(record.AssessmentRollNumber);
            if (_previousRoll.HasValue && _previousRoll.Value != currentRoll)
            {
                //seems to be a chance of duplicates within data.  if we have already dealt with a folio, then we warn and don't add
                if (_sale != null &&  _folios.Contains(_sale.Folio))
                {
                    log.Warn(string.Format("Folio {0} duplicate found on line {1}", _sale.Folio, lineIndex));
                }
                else
                {
                    //write the values
                    if (_sale != null)
                    {
                        SalesList.Add(_sale);
                        //add folio
                        _folios.Add(_sale.Folio);
                    }

                    if (_assessment != null)
                    {
                        AssessmentList.Add(_assessment);
                    }

                    if (_legal != null)
                    {
                        LegalList.Add(_legal);
                    }

                    if (_owner != null)
                    {
                        OwnerList.Add(_owner);
                    }

                    if (_tax != null)
                    {
                        TaxList.Add(_tax);
                    }
                }

                //clear them out...start again
                _sale = null;
                _assessment = null;
                _legal = null;
                _owner = null;
                _tax = null;

                _ownerRecordNos.Clear();
                _taxRecords.Clear();

                _previousRoll = currentRoll;
            }

            //create new records...at most one record for a folio for each entity
            string recordCode = record.GroupCode + record.RecordNo;
            switch (recordCode)
            {
                case Constants.RecordCodeSales:
                    if (_sale != null)
                    {
                        log.Warn(string.Format("there should be only one sale record for folio={0}", folio));
                    }

                    _sale = new Sale();
                    FillSale(_sale, BCASession.ParseRecord<SalesData>(line));
                    break;

                case Constants.RecordCodeLegalDescriptionFixed:
                    if (_legal != null)
                    {
                        log.Warn(string.Format("there should be only one legal record for folio={0}", folio));
                    }

                    _legal = new Legal();
                    FillLegal(_legal, BCASession.ParseRecord<FixedLegalDescription>(line));
                    break;

                case Constants.RecordCodeLegalFreeform1:
                case Constants.RecordCodeLegalFreeform2:
                case Constants.RecordCodeLegalFreeform3:
                    //legal should already exist from the fixed line
                    if (_legal == null)
                    {
                        throw new Exception(string.Format("Cannot apply legal extended values to legal object folio={0}", folio));
                    }

                    FillLegal(_legal, BCASession.ParseRecord<FreeFormLegalDescription>(line));
                    break;

                case Constants.RecordCodeAssessmentValue:
                    if (_assessment != null)
                    {
                        log.Warn(string.Format("there should be only one assessment record for folio={0}", folio));
                    }

                    _assessment = new Assessment();
                    FillAssessment(_assessment, BCASession.ParseRecord<Valuation>(line));
                    break;

                case Constants.RecordCodeAssessmentExtended:
                    //assessment extended...set values to assessment
                    //assessment should already exist from the value line
                    if (_assessment == null)
                    {
                        throw new Exception(string.Format("Cannot apply assessment extended values to assessment object for folio={0}", folio));
                    }

                    FillAssessment(_assessment, BCASession.ParseRecord<ExtendedValuation>(line));
                    break;

                case Constants.RecordCodeMunicipalTax:
                case Constants.RecordCodeSchoolTax:
                    //tax rows read twice
                    if (_taxRecords.Count == 0)
                    {
                        _tax = new Tax();
                        _taxRecords.Add(record.RecordNo);
                    }
                    else if (_taxRecords.Any(a => a == recordCode))
                    {
                        //tax read 2 times...make sure record unique
                        log.Warn(string.Format("Tax record recordno={0} already read for folio={1}", record.RecordNo, currentRoll));
                    }

                    FillTax(_tax, BCASession.ParseRecord<TaxableValues>(line));
                    break;
            }

            //check for owner records
            if (record.GroupCode == Constants.GroupCodeOwner && new List<string>() { "1", "2", "3" }.Any(a => a == record.RecordNo))
            {
                if (_ownerRecordNos.Count == 0)
                {
                    _owner = new Owner();
                    _ownerRecordNos.Add(record.RecordNo);
                }

                else if (_ownerRecordNos.Any(a => a == record.RecordNo))
                {
                    //owner read 3 times...make sure record unique
                    log.Warn(string.Format("Owner record recordno={0} already read for folio={1}", record.RecordNo, folio));
                }

                FillOwner(_owner, line);
            }

            //check for additional owner records
            if (int.Parse(record.GroupCode) >= 101 && int.Parse(record.GroupCode) <= 162)
            {
                //multiple additional owners per folio
                if (record.RecordNo == "1")
                {
                    //signifies new record
                    _additionalOwner = new AdditionalOwner();
                    AdditionalOwnerList.Add(_additionalOwner);
                }
                FillAdditionalOwner(_additionalOwner, line);

                if (_owner == null)
                {
                    log.Warn(string.Format("there should be an owner before additional owner for folio={0}", folio));
                }
                else
                {
                    //flag as more owners
                    _owner.MoreOwners = "Yes";
                }
            }
        }

        #region methods for filling objects from structure

        /// <summary>
        /// Fills the sale.
        /// </summary>
        /// <param name="sale">The sale.</param>
        /// <param name="saleStructure">The sale structure.</param>
        private void FillSale(Sale sale, SalesData saleStructure)
        {
            sale.Folio = NullableParser.ParseString(saleStructure.Jurisdiction + saleStructure.AssessmentRollNumber);
            sale.Jurisdiction = NullableParser.ParseInt(saleStructure.Jurisdiction);
            sale.Roll = NullableParser.ParseInt(saleStructure.AssessmentRollNumber);
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
        }

        /// <summary>
        /// Fills the legal.
        /// </summary>
        /// <param name="legal">The legal.</param>
        /// <param name="legalStruct">The legal struct.</param>
        private void FillLegal(Legal legal, FixedLegalDescription legalStruct)
        {
            legal.Folio = NullableParser.ParseString(legalStruct.Jurisdiction + legalStruct.AssessmentRollNumber);
            legal.Jurisdiction = NullableParser.ParseInt(legalStruct.Jurisdiction);
            legal.Roll = NullableParser.ParseInt(legalStruct.AssessmentRollNumber);
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
            legal.StreetDirection = NullableParser.ParseString(legalStruct.StreetDirection);
            legal.StreetName = NullableParser.ParseString(legalStruct.StreetName);
            legal.PID = NullableParser.ParseString(legalStruct.PropertyIdentificationNumber);

            //sometimes lot size doesn't come in.  if not, ignore rest of fields
            int lotSizeKey;
            if (int.TryParse(legalStruct.LotSizeKey, out lotSizeKey))
            {
                legal.LotSizeCode = NullableParser.TryParseInt(legalStruct.LotSizeKey).Value;

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
            }
        }

        /// <summary>
        /// Fills the legal.
        /// </summary>
        /// <param name="legal">The legal.</param>
        /// <param name="legalStruct">The legal struct.</param>
        private void FillLegal(Legal legal, FreeFormLegalDescription legalStruct)
        {
            string recordCode = legalStruct.GroupCode + legalStruct.RecordNo;
            switch (recordCode)
            {
                case Constants.RecordCodeLegalFreeform1:
                    legal.Description1 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description2 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description3 = NullableParser.ParseString(legalStruct.MHR);
                    break;

                case Constants.RecordCodeLegalFreeform2:
                    legal.Description4 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description5 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description6 = NullableParser.ParseString(legalStruct.MHR);
                    break;

                case Constants.RecordCodeLegalFreeform3:
                    legal.Description7 = NullableParser.ParseString(legalStruct.FreeForm1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description8 = NullableParser.ParseString(legalStruct.FreeForm2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    legal.Description9 = NullableParser.ParseString(legalStruct.MHR);
                    break;
            }
        }

        /// <summary>
        /// Fills the owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="line">The line.</param>
        private void FillOwner(Owner owner, string line)
        {
            BCARecord record = BCASession.ParseRecord<BCARecord>(line);

            switch (record.RecordNo)
            {
                case "1":
                    Ownership ownerStruct = BCASession.ParseRecord<Ownership>(line);
                    owner.Folio = NullableParser.ParseString(ownerStruct.Jurisdiction + ownerStruct.AssessmentRollNumber);
                    owner.Jurisdiction = NullableParser.ParseInt(ownerStruct.Jurisdiction);
                    owner.Roll = NullableParser.ParseInt(ownerStruct.AssessmentRollNumber);
                    owner.Action = NullableParser.TryParseInt(ownerStruct.ActionCode).GetValueOrDefault(0);

                    owner.Surname1 = NullableParser.ParseString(ownerStruct.Surname1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName1 = NullableParser.ParseString(ownerStruct.FirstName1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial1 = NullableParser.ParseString(ownerStruct.Initial1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Surname2 = NullableParser.ParseString(ownerStruct.Surname2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName2 = NullableParser.ParseString(ownerStruct.FirstName2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial2 = NullableParser.ParseString(ownerStruct.Initial2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.PostalCode = NullableParser.ParseString(ownerStruct.PostalCode);
                    owner.MoreOwners = "No";
                    break;

                case "2":
                    OwnershipAddress ownerExtendedStruct2 = BCASession.ParseRecord<OwnershipAddress>(line);
                    owner.Address1 = NullableParser.ParseString(ownerExtendedStruct2.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address2 = NullableParser.ParseString(ownerExtendedStruct2.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address3 = NullableParser.ParseString(ownerExtendedStruct2.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;

                case "3":
                    OwnershipAddress ownerExtendedStruct3 = BCASession.ParseRecord<OwnershipAddress>(line);
                    owner.Address4 = NullableParser.ParseString(ownerExtendedStruct3.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address5 = NullableParser.ParseString(ownerExtendedStruct3.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address6 = NullableParser.ParseString(ownerExtendedStruct3.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;
            }
        }

        /// <summary>
        /// Fills the additional owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="line">The line.</param>
        private void FillAdditionalOwner(AdditionalOwner owner, string line)
        {
            BCARecord record = BCASession.ParseRecord<BCARecord>(line);

            switch (record.RecordNo)
            {
                case "1":
                    Ownership ownerStruct = BCASession.ParseRecord<Ownership>(line);
                    owner.Folio = NullableParser.ParseString(ownerStruct.Jurisdiction + ownerStruct.AssessmentRollNumber);
                    owner.GroupCode = NullableParser.ParseString(ownerStruct.GroupCode);
                    owner.FolioGroup = NullableParser.ParseString(ownerStruct.Jurisdiction + ownerStruct.AssessmentRollNumber + owner.GroupCode);

                    owner.Jurisdiction = NullableParser.ParseInt(ownerStruct.Jurisdiction);
                    owner.Roll = NullableParser.ParseInt(ownerStruct.AssessmentRollNumber);
                    owner.Action = NullableParser.TryParseInt(ownerStruct.ActionCode).GetValueOrDefault(0);

                    owner.Surname1 = NullableParser.ParseString(ownerStruct.Surname1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName1 = NullableParser.ParseString(ownerStruct.FirstName1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial1 = NullableParser.ParseString(ownerStruct.Initial1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Surname2 = NullableParser.ParseString(ownerStruct.Surname2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.FirstName2 = NullableParser.ParseString(ownerStruct.FirstName2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Initial2 = NullableParser.ParseString(ownerStruct.Initial2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.PostalCode = NullableParser.ParseString(ownerStruct.PostalCode);
                    break;

                case "2":
                    OwnershipAddress ownerExtendedStruct2 = BCASession.ParseRecord<OwnershipAddress>(line);
                    owner.Address1 = NullableParser.ParseString(ownerExtendedStruct2.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address2 = NullableParser.ParseString(ownerExtendedStruct2.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address3 = NullableParser.ParseString(ownerExtendedStruct2.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;

                case "3":
                    OwnershipAddress ownerExtendedStruct3 = BCASession.ParseRecord<OwnershipAddress>(line);
                    owner.Address4 = NullableParser.ParseString(ownerExtendedStruct3.Address1.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address5 = NullableParser.ParseString(ownerExtendedStruct3.Address2.Replace(",", string.Empty).Replace("\"", string.Empty));
                    owner.Address6 = NullableParser.ParseString(ownerExtendedStruct3.Address3.Replace(",", string.Empty).Replace("\"", string.Empty));
                    break;
            }
        }

        /// <summary>
        /// Fills the assessment.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <param name="valuation">The valuation.</param>
        private void FillAssessment(Assessment assessment, Valuation valuation)
        {
            assessment.Folio = NullableParser.ParseString(valuation.Jurisdiction + valuation.AssessmentRollNumber);
            assessment.Jurisdiction = NullableParser.ParseInt(valuation.Jurisdiction);
            assessment.Roll = NullableParser.ParseInt(valuation.AssessmentRollNumber);
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
        }

        /// <summary>
        /// Fills the assessment.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <param name="valuation">The valuation.</param>
        private void FillAssessment(Assessment assessment, ExtendedValuation valuation)
        {
            assessment.ExtendedLand1 = NullableParser.TryParseDouble(valuation.LandValue1).GetValueOrDefault(0);
            assessment.ExtendedImprovement1 = NullableParser.TryParseDouble(valuation.ImproveValue1).GetValueOrDefault(0);

            assessment.ExtendedLand2 = NullableParser.TryParseDouble(valuation.LandValue2).GetValueOrDefault(0);
            assessment.ExtendedImprovement2 = NullableParser.TryParseDouble(valuation.ImproveValue2).GetValueOrDefault(0);

            assessment.ExtendedLand3 = NullableParser.TryParseDouble(valuation.LandValue3).GetValueOrDefault(0);
            assessment.ExtendedImprovement3 = NullableParser.TryParseDouble(valuation.ImproveValue3).GetValueOrDefault(0);
        }

        /// <summary>
        /// Fills the tax.
        /// </summary>
        /// <param name="tax">The tax.</param>
        /// <param name="taxableValues">The taxable values.</param>
        private void FillTax(Tax tax, TaxableValues taxableValues)
        {
            tax.Folio = NullableParser.ParseString(taxableValues.Jurisdiction + taxableValues.AssessmentRollNumber);
            tax.Jurisdiction = NullableParser.ParseInt(taxableValues.Jurisdiction);
            tax.Roll = NullableParser.ParseInt(taxableValues.AssessmentRollNumber);
            tax.Action = NullableParser.TryParseInt(taxableValues.ActionCode).GetValueOrDefault(0);

            string recordCode = taxableValues.GroupCode + taxableValues.RecordNo;
            switch (recordCode)
            {
                case Constants.RecordCodeMunicipalTax:
                    tax.MunicipalClassCode1 = NullableParser.TryParseInt(taxableValues.ClassCode1).GetValueOrDefault(0);
                    tax.MunicipalClassDescription1 = _taxClassMap[tax.MunicipalClassCode1];
                    tax.MunicipalGrossLand1 = NullableParser.TryParseDouble(taxableValues.GrossLand1).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement1 = NullableParser.TryParseDouble(taxableValues.GrossImprovement1).GetValueOrDefault(0);
                    tax.MunicipalExemptLand1 = NullableParser.TryParseDouble(taxableValues.ExemptLand1).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement1 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement1).GetValueOrDefault(0);

                    tax.MunicipalClassCode2 = NullableParser.TryParseInt(taxableValues.ClassCode2).GetValueOrDefault(0);
                    tax.MunicipalClassDescription2 = _taxClassMap[tax.MunicipalClassCode2];
                    tax.MunicipalGrossLand2 = NullableParser.TryParseDouble(taxableValues.GrossLand2).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement2 = NullableParser.TryParseDouble(taxableValues.GrossImprovement2).GetValueOrDefault(0);
                    tax.MunicipalExemptLand2 = NullableParser.TryParseDouble(taxableValues.ExemptLand2).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement2 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement2).GetValueOrDefault(0);

                    tax.MunicipalClassCode3 = NullableParser.TryParseInt(taxableValues.ClassCode3).GetValueOrDefault(0);
                    tax.MunicipalClassDescription3 = _taxClassMap[tax.MunicipalClassCode3];
                    tax.MunicipalGrossLand3 = NullableParser.TryParseDouble(taxableValues.GrossLand3).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement3 = NullableParser.TryParseDouble(taxableValues.GrossImprovement3).GetValueOrDefault(0);
                    tax.MunicipalExemptLand3 = NullableParser.TryParseDouble(taxableValues.ExemptLand3).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement3 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement3).GetValueOrDefault(0);

                    tax.MunicipalClassCode4 = NullableParser.TryParseInt(taxableValues.ClassCode4).GetValueOrDefault(0);
                    tax.MunicipalClassDescription4 = _taxClassMap[tax.MunicipalClassCode4];
                    tax.MunicipalGrossLand4 = NullableParser.TryParseDouble(taxableValues.GrossLand4).GetValueOrDefault(0);
                    tax.MunicipalGrossImprovement4 = NullableParser.TryParseDouble(taxableValues.GrossImprovement4).GetValueOrDefault(0);
                    tax.MunicipalExemptLand4 = NullableParser.TryParseDouble(taxableValues.ExemptLand4).GetValueOrDefault(0);
                    tax.MunicipalExemptImprovement4 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement4).GetValueOrDefault(0);
                    break;

                case Constants.RecordCodeSchoolTax:
                    tax.SchoolClassCode1 = NullableParser.TryParseInt(taxableValues.ClassCode1).GetValueOrDefault(0);
                    tax.SchoolClassDescription1 = _taxClassMap[tax.SchoolClassCode1];
                    tax.SchoolGrossLand1 = NullableParser.TryParseDouble(taxableValues.GrossLand1).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement1 = NullableParser.TryParseDouble(taxableValues.GrossImprovement1).GetValueOrDefault(0);
                    tax.SchoolExemptLand1 = NullableParser.TryParseDouble(taxableValues.ExemptLand1).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement1 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement1).GetValueOrDefault(0);

                    tax.SchoolClassCode2 = NullableParser.TryParseInt(taxableValues.ClassCode2).GetValueOrDefault(0);
                    tax.SchoolClassDescription2 = _taxClassMap[tax.SchoolClassCode2];
                    tax.SchoolGrossLand2 = NullableParser.TryParseDouble(taxableValues.GrossLand2).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement2 = NullableParser.TryParseDouble(taxableValues.GrossImprovement2).GetValueOrDefault(0);
                    tax.SchoolExemptLand2 = NullableParser.TryParseDouble(taxableValues.ExemptLand2).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement2 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement2).GetValueOrDefault(0);

                    tax.SchoolClassCode3 = NullableParser.TryParseInt(taxableValues.ClassCode3).GetValueOrDefault(0);
                    tax.SchoolClassDescription3 = _taxClassMap[tax.SchoolClassCode3];
                    tax.SchoolGrossLand3 = NullableParser.TryParseDouble(taxableValues.GrossLand3).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement3 = NullableParser.TryParseDouble(taxableValues.GrossImprovement3).GetValueOrDefault(0);
                    tax.SchoolExemptLand3 = NullableParser.TryParseDouble(taxableValues.ExemptLand3).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement3 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement3).GetValueOrDefault(0);

                    tax.SchoolClassCode4 = NullableParser.TryParseInt(taxableValues.ClassCode4).GetValueOrDefault(0);
                    tax.SchoolClassDescription4 = _taxClassMap[tax.SchoolClassCode4];
                    tax.SchoolGrossLand4 = NullableParser.TryParseDouble(taxableValues.GrossLand4).GetValueOrDefault(0);
                    tax.SchoolGrossImprovement4 = NullableParser.TryParseDouble(taxableValues.GrossImprovement4).GetValueOrDefault(0);
                    tax.SchoolExemptLand4 = NullableParser.TryParseDouble(taxableValues.ExemptLand4).GetValueOrDefault(0);
                    tax.SchoolExemptImprovement4 = NullableParser.TryParseDouble(taxableValues.ExemptImprovement4).GetValueOrDefault(0);
                    break;
            }

        }

        /// <summary>
        /// Parses the sale date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static DateTime? ParseSaleDate(string date)
        {
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);

            DateTime yearResult;
            if (!DateTime.TryParseExact(year, "yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out yearResult))
            {
                return null;
            }

            if (month == "00") month = "01";
            DateTime monthResult;
            if (!DateTime.TryParseExact(month, "mm", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out monthResult))
            {
                return null;
            }

            DateTime retVal = DateTime.ParseExact(string.Format("01/{0}/{1}", month, year), "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            return retVal;
        }

        #endregion

    }
}

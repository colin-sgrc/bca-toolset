using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDKB.BCAAImport
{
    public interface IBCAAObjectBuilder
    {
        List<Sale> SalesList { get; set; }
        List<Legal> LegalList { get; set; }
        List<Assessment> AssessmentList { get; set; }
        List<Owner> OwnerList { get; set; }
        List<AdditionalOwner> AdditionalOwnerList { get; set; }
        List<Tax> TaxList { get; set; }

    }
}

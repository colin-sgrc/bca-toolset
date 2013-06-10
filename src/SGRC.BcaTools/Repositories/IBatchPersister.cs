using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDKB.BCAAImport
{
    public interface IBatchPersister<T> where T : IDomainObject
    {
        void PersistMonthlyDatFile(List<T> domainObjects);
        void PersistYearlyDatFile(List<T> domainObjects);
    }

}

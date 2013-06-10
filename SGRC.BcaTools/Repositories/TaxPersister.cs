using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class TaxPersister : IBatchPersister<Tax>
    {
        public void PersistMonthlyDatFile(List<Tax> list)
        {
            //taxes
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //taxes to be added
                var newTaxes = list.Where(a => a.Action == 2);
                foreach (Tax tax in newTaxes)
                {
                    Tax toAdd = session.Get<Tax>(tax.Folio);
                    if (toAdd == null)
                    {
                        session.Insert(tax);
                        BCAACommon.Log.Debug(string.Format("ADDED: Tax {0}", tax.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of tax {0} where already exists in database", toAdd.Folio));
                    }
                }

                //update
                var updatedTaxes = list.Where(a => a.Action == 3);
                foreach (Tax tax in updatedTaxes)
                {
                    Tax toUpdate = session.Get<Tax>(tax.Folio);
                    if (toUpdate != null)
                    {
                        session.Update(tax);
                        BCAACommon.Log.Debug(string.Format("UPDATED: Tax {0}", tax.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted update of tax {0} where it doesn't exist in database", tax.Folio));
                    }
                }

                //delete
                var deletedTaxes = list.Where(a => a.Action == 1);
                foreach (Tax tax in deletedTaxes)
                {
                    Tax toDelete = session.Get<Tax>(tax.Folio);
                    if (toDelete != null)
                    {
                        session.Delete(tax);
                        BCAACommon.Log.Debug(string.Format("DELETED: Tax {0}", tax.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of tax {0} where it doesn't exist in database", tax.Folio));
                    }
                }
                transaction.Commit();
            }
        }

        public void PersistYearlyDatFile(List<Tax> list)
        {
            //tax
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete Tax t").ExecuteUpdate(); 
                
                foreach (Tax tax in list)
                {
                    session.Insert(tax);
                    BCAACommon.Log.Debug(string.Format("ADDED: Tax {0}", tax.Folio));
                }
                transaction.Commit();
            }
        }
    }
}

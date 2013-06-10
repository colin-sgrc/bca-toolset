using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class SalePersister : IBatchPersister<Sale>
    {
        public void PersistYearlyDatFile(List<Sale> list)
        {
            //sales
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete Sale s").ExecuteUpdate(); 
                
                foreach (Sale sale in list)
                {
                    session.Insert(sale);
                    BCAACommon.Log.Debug(string.Format("ADDED: Sale {0}", sale.Folio));
                }
                transaction.Commit();
            }
        }

        public void PersistMonthlyDatFile(List<Sale> list)
        {
            //sales
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //sales to be added
                var newSales = list.Where(a => a.Action == 2);
                foreach (Sale sale in newSales)
                {
                    Sale toAdd = session.Get<Sale>(sale.Folio);
                    if (toAdd == null)
                    {
                        object id = session.Insert(sale);
                        BCAACommon.Log.Debug(string.Format("ADDED: Sale {0}", sale.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of sale {0} where already exists in database", toAdd.Folio));
                    }
                }

                var updatedSales = list.Where(a => a.Action == 3);
                foreach (Sale sale in updatedSales)
                {
                    Sale toUpdate = session.Get<Sale>(sale.Folio);
                    if (toUpdate != null)
                    {
                        session.Update(sale);
                        BCAACommon.Log.Debug(string.Format("UPDATED: Sale {0}", sale.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted update of sale {0} where it doesn't exist in database", sale.Folio));
                    }
                }

                var deletedSales = list.Where(a => a.Action == 1);
                foreach (Sale sale in deletedSales)
                {
                    Sale toDelete = session.Get<Sale>(sale.Folio);
                    if (toDelete != null)
                    {
                        session.Delete(sale);
                        BCAACommon.Log.Debug(string.Format("DELETED: Sale {0}", sale.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of sale {0} where it doesn't exist in database", sale.Folio));
                    }
                }
                transaction.Commit();
            }
        }

    }
}

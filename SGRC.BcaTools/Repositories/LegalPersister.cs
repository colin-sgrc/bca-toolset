using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class LegalPersister : IBatchPersister<Legal>
    {
        public void PersistMonthlyDatFile(List<Legal> list)
        {
            //legals
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //legals to be added
                var newLegals = list.Where(a => a.Action == 2);
                foreach (Legal legal in newLegals)
                {
                    Legal toAdd = session.Get<Legal>(legal.Folio);
                    if (toAdd == null)
                    {
                        session.Insert(legal);
                        BCAACommon.Log.Debug(string.Format("ADDED: Legal {0}", legal.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of legal {0} where already exists in database", toAdd.Folio));
                    }
                }

                //updates
                var updatedLegals = list.Where(a => a.Action == 3);
                foreach (Legal legal in updatedLegals)
                {
                    Legal toUpdate = session.Get<Legal>(legal.Folio);
                    if (toUpdate != null)
                    {
                        session.Update(legal);
                        BCAACommon.Log.Debug(string.Format("UPDATED: Legal {0}", legal.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted update of legal {0} where it doesn't exist in database", toUpdate));
                    }
                }

                //deletes
                var deletedLegals = list.Where(a => a.Action == 1);
                foreach (Legal legal in deletedLegals)
                {
                    Legal toDelete = session.Get<Legal>(legal.Folio);
                    if (toDelete != null)
                    {
                        session.Delete(legal);
                        BCAACommon.Log.Debug(string.Format("DELETED: Legal {0}", legal.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of legal {0} where it doesn't exist in database", legal.Folio));
                    }
                }
                transaction.Commit();
            }
        }

        public void PersistYearlyDatFile(List<Legal> list)
        {
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete Legal l").ExecuteUpdate();

                foreach (Legal legal in list)
                {
                    session.Insert(legal);
                    BCAACommon.Log.Debug(string.Format("ADDED: Legal {0}", legal.Folio));
                }
                transaction.Commit();
            }
        }
    }
}

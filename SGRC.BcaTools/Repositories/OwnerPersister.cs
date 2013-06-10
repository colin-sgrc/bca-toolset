using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class OwnerPersister : IBatchPersister<Owner>
    {
        public void PersistMonthlyDatFile(List<Owner> list)
        {
            //owners
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //owners to be added
                var newOwners = list.Where(a => a.Action == 2);
                foreach (Owner owner in newOwners)
                {
                    Owner toAdd = session.Get<Owner>(owner.Folio);
                    if (toAdd == null)
                    {
                        session.Insert(owner);
                        BCAACommon.Log.Debug(string.Format("ADDED: Owner {0}", owner.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of owner {0} where already exists in database", toAdd.Folio));
                    }
                }

                //updates
                var updatedOwners = list.Where(a => a.Action == 3);
                foreach (Owner owner in updatedOwners)
                {
                    Owner toUpdate = session.Get<Owner>(owner.Folio);
                    if (toUpdate != null)
                    {
                        session.Update(owner);
                        BCAACommon.Log.Debug(string.Format("UPDATED: Owner {0}", owner.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted update of owner {0} where it doesn't exist in database", owner.Folio));
                    }
                }

                //deletes
                var deletedOwners = list.Where(a => a.Action == 1);
                foreach (Owner owner in deletedOwners)
                {
                    Owner toDelete = session.Get<Owner>(owner.Folio);
                    if (toDelete != null)
                    {
                        session.Delete(owner);
                        BCAACommon.Log.Debug(string.Format("DELETED: Owner {0}", owner.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of owner {0} where it doesn't exist in database", owner.Folio));
                    }
                }
                transaction.Commit();
            }
        }

        public void PersistYearlyDatFile(List<Owner> list)
        {
            //owners
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete Owner o").ExecuteUpdate();

                foreach (Owner owner in list)
                {
                    session.Insert(owner);
                    BCAACommon.Log.Debug(string.Format("ADDED: Owner {0}", owner.Folio));
                }
                transaction.Commit();
            }
        }

    }
}

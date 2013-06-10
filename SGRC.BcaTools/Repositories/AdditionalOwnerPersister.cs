using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class AdditionalOwnerPersister : IBatchPersister<AdditionalOwner>
    {
        public void PersistMonthlyDatFile(List<AdditionalOwner> list)
        {
            //additionalOwners
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //additionalOwners to be added
                var newAdditionalOwners = list.Where(a => a.Action == 2);
                foreach (AdditionalOwner additionalOwner in newAdditionalOwners)
                {
                    AdditionalOwner toAdd = session.Get<AdditionalOwner>(additionalOwner.FolioGroup);
                    if (toAdd == null)
                    {
                        session.Insert(additionalOwner);
                        BCAACommon.Log.Debug(string.Format("ADDED: AdditionalOwner {0}", additionalOwner.FolioGroup));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of additionalOwner {0} where already exists in database", toAdd.FolioGroup));
                    }
                }

                //update.  these seem to come over items not in the db as well as real updates
                var updatedAdditionalOwners = list.Where(a => a.Action == 3);
                foreach (AdditionalOwner additionalOwner in updatedAdditionalOwners)
                {
                    AdditionalOwner toUpdate = session.Get<AdditionalOwner>(additionalOwner.FolioGroup);
                    if (toUpdate != null)
                    {
                        session.Update(additionalOwner);
                        BCAACommon.Log.Debug(string.Format("ADDED: AdditionalOwner {0}", additionalOwner.FolioGroup));
                    }
                    else
                    {
                        //just add it
                        session.Insert(additionalOwner);
                        BCAACommon.Log.Debug(string.Format("ADDED: AdditionalOwner {0}", additionalOwner.FolioGroup));
                    }
                }

                //delete
                var deletedAdditionalOwners = list.Where(a => a.Action == 1);
                foreach (AdditionalOwner additionalOwner in deletedAdditionalOwners)
                {
                    AdditionalOwner toDelete = session.Get<AdditionalOwner>(additionalOwner.FolioGroup);
                    if (toDelete != null)
                    {
                        session.Delete(additionalOwner);
                        BCAACommon.Log.Debug(string.Format("DELETED: AdditionalOwner {0}", additionalOwner.FolioGroup));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of additionalOwner {0} where it doesn't exist in database", additionalOwner.FolioGroup));
                    }
                }
                transaction.Commit();
            }
        }

        public void PersistYearlyDatFile(List<AdditionalOwner> list)
        {
            //additional owners
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete AdditionalOwner o").ExecuteUpdate();

                foreach (AdditionalOwner additionalOwner in list)
                {
                    session.Insert(additionalOwner);
                    BCAACommon.Log.Debug(string.Format("ADDED: AdditionalOwner {0}", additionalOwner.FolioGroup));
                }
                transaction.Commit();
            }
        }
    }
}

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
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;

namespace SGRC.BCATools
{

    /// <summary>
    /// Responsible for persisting domain objects to data store.  Data store abstracted away thanks to NHibernate
    /// </summary>
    public class BCAPersister
    {
        /// <summary>
        /// Persists the monthly dat file.
        /// </summary>
        /// <param name="loader">The loader.</param>
        public void PersistMonthlyDatFile(IBCAObjectContainer loader)
        {
            BCASession.Current.Log.Info("Beginning to persist data to database...");
            //force close as to reload connection (and dest db might be different from last import)
            NHibernateHelper.CloseSessionFactory();

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //figure out the items to be deleted.  these exist on the sales record and apply to all object types
            List<string> foliosToDelete = loader.SalesList.Where(a => a.Action == 1).Select(a => a.Folio).ToList();

            PersistMonthly<Sale>(loader.SalesList, foliosToDelete);
            PersistMonthly<Legal>(loader.LegalList, foliosToDelete);
            PersistMonthly<Assessment>(loader.AssessmentList, foliosToDelete);
            PersistMonthly<Tax>(loader.TaxList, foliosToDelete);
            PersistMonthly<Owner>(loader.OwnerList, foliosToDelete);
            PersistMonthly<AdditionalOwner>(loader.AdditionalOwnerList, foliosToDelete);

            stopwatch.Stop();
            BCASession.Current.Log.Info(string.Format("Monthly import complete in {0:#0.0#} minutes", stopwatch.Elapsed.TotalMinutes));
        }

        /// <summary>
        /// Persists the monthly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="foliosToDelete">The folios to delete.</param>
        private void PersistMonthly<T>(IList<T> list, List<string> foliosToDelete) where T : IDomainObject
        {
            //grab the id and entity name.  assume id is single field
            var persistentClass = NHibernateHelper.GetCurrentConfiguration().GetClassMapping(typeof(T));
            string identifierName = persistentClass.IdentifierProperty.Name;
            string entityName = persistentClass.NodeName;

            int inserted = 0;
            int updated = 0;
            int deleted = 0;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //items to be added
                var newItems = list.Where(a => a.Action == 2);
                foreach (T newItem in newItems)
                {
                    object id = typeof(T).GetProperty(identifierName).GetValue(newItem, null);
                    T toAdd = session.Get<T>(id);

                    if (toAdd == null)
                    {
                        session.Insert(newItem);
                        BCASession.Current.Log.Debug(string.Format("ADDED {0}: {1}", entityName, newItem.Folio));
                        inserted++;
                    }
                    else
                    {
                        session.Update(newItem);
                        BCASession.Current.Log.Debug(string.Format("addition attempt of {0} {1} where already exists.  Performed update instead.", entityName, id));
                        updated++;
                    }
                }

                //updates
                var itemsToUpdate = list.Where(a => a.Action == 3);
                foreach (T item in itemsToUpdate)
                {
                    object id = typeof(T).GetProperty(identifierName).GetValue(item, null);
                    T toUpdate = session.Get<T>(id);
                    if (toUpdate != null)
                    {
                        session.Update(item);
                        BCASession.Current.Log.Debug(string.Format("UPDATED: {0} {1}", entityName, id));
                        updated++;
                    }
                    else
                    {
                        session.Insert(item);
                        inserted++;
                        BCASession.Current.Log.Debug(string.Format("update attempt of {0} {1} where non-existent.  Performed insert instead.", entityName, id));
                    }
                }

                //delete
                if (foliosToDelete.Count > 0)
                {
                    deleted = session.CreateQuery(string.Format("delete from {0} as e where e.Folio IN (:key)", entityName))
                        .SetParameterList("key", foliosToDelete.ToArray())
                        .ExecuteUpdate();
                }

                transaction.Commit();
            }

            //summarize
            BCASession.Current.Log.Info(string.Format("{0} import complete in {4:#0.0#} minutes: Added: {1}, Updated: {2}, Deleted {3}", entityName, inserted, updated, deleted, stopwatch.Elapsed.TotalMinutes));
        }

        /// <summary>
        /// Persists the yearly dat file.
        /// </summary>
        /// <param name="loader">The manager.</param>
        public void PersistYearlyDatFile(IBCAObjectContainer loader)
        {
            BCASession.Current.Log.Info("Beginning to persist data to database...");
            //force close as to reload connection (and dest db might be different from last import)
            NHibernateHelper.CloseSessionFactory();

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            RemoveRecords<Sale>();
            RemoveRecords<Legal>();
            RemoveRecords<Assessment>();
            RemoveRecords<Tax>();
            RemoveRecords<Owner>();
            RemoveRecords<AdditionalOwner>();

            PersistYearly<Sale>(loader.SalesList);
            PersistYearly<Legal>(loader.LegalList);
            PersistYearly<Assessment>(loader.AssessmentList);
            PersistYearly<Tax>(loader.TaxList);
            PersistYearly<Owner>(loader.OwnerList);
            PersistYearly<AdditionalOwner>(loader.AdditionalOwnerList);

            stopwatch.Stop();
            BCASession.Current.Log.Info(string.Format("Yearly import complete in {0:#0.0#} minutes", stopwatch.Elapsed.TotalMinutes));
        }

        /// <summary>
        /// Removes the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RemoveRecords<T>()
        {
            var persistentClass = NHibernateHelper.GetCurrentConfiguration().GetClassMapping(typeof(T));
            string entityName = persistentClass.NodeName;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //remove all records first
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                stopwatch.Reset();
                stopwatch.Start();

                int deleted = session.CreateQuery(string.Format("delete {0} as e", entityName)).ExecuteUpdate();
                BCASession.Current.Log.Info(string.Format("Deleted {2} {1} records in {0:#0.0#} minutes", stopwatch.Elapsed.TotalMinutes, entityName, deleted));

                transaction.Commit();
            }
            stopwatch.Stop();
        }

        /// <summary>
        /// Persists the yearly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        private void PersistYearly<T>(IList<T> list)
        {
            var persistentClass = NHibernateHelper.GetCurrentConfiguration().GetClassMapping(typeof(T));
            string identifierName = persistentClass.IdentifierProperty.Name;
            string entityName = persistentClass.NodeName;

            BCASession.Current.Log.Info(string.Format("Performing {0} import...", entityName));

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (T item in list)
                {
                    string id = typeof(T).GetProperty(identifierName).GetValue(item, null).ToString();

                    session.Insert(item);
                    BCASession.Current.Log.Debug(string.Format("ADDED: {0} {1}", entityName, id));
                }
                transaction.Commit();
            }
            //summarize
            BCASession.Current.Log.Info(string.Format("{0} import complete in {2:#0.0#} minutes: Added: {1}", entityName, list.Count, stopwatch.Elapsed.TotalMinutes));
        }


    }
}

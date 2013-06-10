using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace RDKB.BCAAImport
{
    public class AssessmentPersister : IBatchPersister<Assessment>
    {
        public void PersistMonthlyDatFile(List<Assessment> list)
        {
            //assessments
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //assessments to be added
                var newAssessments = list.Where(a => a.Action == 2);
                foreach (Assessment assessment in newAssessments)
                {
                    Assessment toAdd = session.Get<Assessment>(assessment.Folio);
                    if (toAdd == null)
                    {
                        session.Insert(assessment);
                        BCAACommon.Log.Debug(string.Format("ADDED: Assessment {0}", assessment.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted addition of assessment {0} where already exists in database", toAdd.Folio));
                    }
                }

                //updates
                var updatedAssessments = list.Where(a => a.Action == 3);
                foreach (Assessment assessment in updatedAssessments)
                {
                    Assessment toUpdate = session.Get<Assessment>(assessment.Folio);
                    if (toUpdate != null)
                    {
                        session.Update(assessment);
                        BCAACommon.Log.Debug(string.Format("UPDATED: Assessment {0}", assessment.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted update of assessment {0} where it doesn't exist in database", toUpdate.Folio));
                    }
                }

                //deletes
                var deletedAssessments = list.Where(a => a.Action == 1);
                foreach (Assessment assessment in deletedAssessments)
                {
                    Assessment toDelete = session.Get<Assessment>(assessment.Folio);
                    if (toDelete != null)
                    {
                        session.Delete(assessment);
                        BCAACommon.Log.Debug(string.Format("DELETED: Assessment {0}", assessment.Folio));
                    }
                    else
                    {
                        BCAACommon.Log.Warn(string.Format("attempted delete of assessment {0} where it doesn't exist in database", assessment.Folio));
                    }
                }
                transaction.Commit();
            }
        }

        public void PersistYearlyDatFile(List<Assessment> list)
        {
            //assessment
            using (IStatelessSession session = NHibernateHelper.GetCurrentStatelessSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete Assessment a").ExecuteUpdate();

                foreach (Assessment assessment in list)
                {
                    session.Insert(assessment);
                    BCAACommon.Log.Debug(string.Format("ADDED: Assessment {0}", assessment.Folio));
                }
                transaction.Commit();
            }
        }
    }
}

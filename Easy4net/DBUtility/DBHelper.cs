using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.EntityManager;
using System.Data;
using Easy4net.Common;

namespace Easy4net.DBUtility
{
    public class DBHelper
    {
        public IDbTransaction trans;
        public EntityManager.EntityManager entityManager;

        public DBHelper()
        {
            entityManager = EntityManagerFactory.CreateEntityManager();
        }

        public static DBHelper getInstance()
        {
            return new DBHelper();
        }

        public int Save<T>(T entity)
        {
            if (trans != null) entityManager.Transaction = trans;
            return entityManager.Save<T>(entity);
        }

        public int Update<T>(T entity)
        {
            if (trans != null) entityManager.Transaction = trans;
            return entityManager.Update<T>(entity);
        }

        public int Remove<T>(T entity)
        {
            if (trans != null) entityManager.Transaction = trans;
            return entityManager.Remove<T>(entity);
        }

        public int Remove<T>(object id) where T : new()
        {
            if (trans != null) entityManager.Transaction = trans;
            return entityManager.Remove<T>(id);
        }

        public List<T> FindAll<T>() where T : new()
        {
            return entityManager.FindAll<T>();
        }
        
        public List<T> FindBySql<T>(string strSql) where T : new()
        {
            return entityManager.FindBySql<T>(strSql);
        }

        public List<T> FindBySql<T>(string strSql, int pageIndex, int pageSize, string order, bool desc) where T : new()
        {
            return entityManager.FindBySql<T>(strSql, pageIndex, pageSize, order, desc);
        }

        public List<T> FindBySql<T>(string strSql, ParamMap param) where T : new()
        {
            return entityManager.FindBySql<T>(strSql, param);
        }

        public T FindById<T>(object id) where T : new()
        {
            return entityManager.FindById<T>(id);
        }

        public List<T> FindByProperty<T>(string propertyName, object propertyValue) where T : new()
        {
            return entityManager.FindByProperty<T>(propertyName, propertyValue);
        }

        public int FindCount<T>() where T : new()
        {
            return entityManager.FindCount<T>();
        }

        public int FindCount<T>(string propertyName, object propertyValue) where T : new()
        {
            return entityManager.FindCount<T>(propertyName, propertyValue);
        }

        public int FindCount<T>(DbCondition condition) where T : new()
        {
            return entityManager.FindCount<T>(condition);
        }

        public List<T> Find<T>(DbCondition condition) where T : new()
        {
            return entityManager.Find<T>(condition);
        }

        /*public List<T> Find<T>(WhereExpression where) where T : new()
        {
            return null;
        }*/

        public void BeginTransaction()
        {
            trans = DbFactory.CreateDbTransaction();
        }

        public void CommitTransaction()
        {
            if (trans != null)
            {
                trans.Commit();
                trans.Dispose();
                trans = null;
            }
        }

        public void RollbackTransaction()
        {
            if (trans != null)
            {
                trans.Rollback();
                trans.Dispose();
                trans = null;
            }
        }

        /*public static int Save<T>(T entity)
        {
            if(trans != null) em.Transaction = trans;
            return em.Save<T>(entity);
        }

        public static int Update<T>(T entity)
        {
            if (trans != null) em.Transaction = trans;
            return em.Update<T>(entity);
        }

        public static int Remove<T>(T entity)
        {
            if (trans != null) em.Transaction = trans;
            return em.Remove<T>(entity);
        }

        public static int Remove<T>(object id) where T : new()
        {
            if (trans != null) em.Transaction = trans;
            return em.Remove<T>(id);
        }

        public static List<T> FindAll<T>() where T : new()
        {
            return em.FindAll<T>();
        }

        public static List<T> FindBySql<T>(string strSql) where T : new()
        {
            return em.FindBySql<T>(strSql);
        }

        public static T FindById<T>(object id) where T : new()
        {
            return em.FindById<T>(id);
        }

        public static void BeginTransaction()
        {
            trans = DbFactory.CreateDbTransaction();
        }

        public static void CommitTransaction()
        {
            if (trans != null)
            {
                trans.Commit();
                trans.Dispose();
                trans = null;
            }
        }

        public static void RollbackTransaction()
        {
            if (trans != null)
            {
                trans.Rollback();
                trans.Dispose();
                trans = null;
            }
        }*/
    }
}

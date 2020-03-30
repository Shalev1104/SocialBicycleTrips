using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

/*
  https://docs.microsoft.com/en-us/xamarin/android/data-cloud/data-access/using-sqlite-orm

  Async
  https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/databases
 */

namespace Dal
{
    public class DbTable<T> where T : new()
    {
        private readonly static SQLiteConnection db;
        private static readonly object locker = new object();

        static DbTable()
        {
            db = DataBase.Instance.Connection;
        }

        public static int CreateTable()
        {
            try
            {
                lock (locker)
                {
                    return Convert.ToInt32(db.CreateTable<T>());
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static int Insert(T entity)
        {
            try
            {
                lock (locker)
                {
                    CreateTable();
                    return db.Insert(entity);
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static int Update(T entity)
        {
            try
            {
                lock (locker)
                {
                    return db.Update(entity);
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static int Delete(T entity)
        {
            try
            {
                lock (locker)
                {
                    return db.Delete(entity);
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static int Delete(int id)
        {
            try
            {
                lock (locker)
                {
                    return db.Delete(id);
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public int Save(T entity)
        {
            lock (locker)
            {
                if ((int)entity.GetType().GetProperty("Id").GetValue(entity, null) != 0)
                {
                    return Update(entity);
                }
                else
                {
                    return Insert(entity);
                }
            }
        }

        // SQL - Insert / Update / Delete
        public static int Execute(string sql)
        {
            try
            {
                lock (locker)
                {
                    return db.Execute(sql);
                }
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static List<T> SelectAll()
        {
            try
            {
                lock (locker)
                {
                    return db.Table<T>().ToList();
                }
            }
            catch (SQLiteException e)
            {
                return null;
            }
        }

        public static IEnumerable<T> SelectAll(bool isEnumerable)
        {
            try
            {
                lock (locker)
                {
                    return db.Table<T>();
                }
            }
            catch (SQLiteException e)
            {
                return null;
            }
        }

        public static int Count()
        {
            try
            {
                return SelectAll().Count;
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static List<T> SelectQuery(string sql)
        {
            try
            {
                lock (locker)
                {
                    return db.Query<T>(sql).ToList();
                }
            }
            catch (SQLiteException e)
            {
                return null;
            }
        }

        public static int CountQuery(string sql)
        {
            try
            {
                return SelectQuery(sql).Count;
            }
            catch (SQLiteException e)
            {
                return -1;
            }
        }

        public static object Get(int id)
        {
            try
            {
                lock (locker)
                {
                    return db.Get<T>(id);
                }
            }
            catch (SQLiteException e)
            {
                return null;
            }
        }

        public List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return query.ToList<T>();
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using SQLite;

namespace Dal
{
    public class BaseDB
    {
        protected string dbName;
        protected static string documentsPath;
        protected static string dbPath;
        protected static SQLiteConnection connection;
        protected CreateTableResult result;
        private static BaseDB instance = null;
        private static readonly object padlock = new object();

        public BaseDB()
        {
            dbName = "name.db";

            documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            dbPath = System.IO.Path.Combine(documentsPath, dbName);

            connection = new SQLiteConnection(dbPath);

        }

        public SQLiteConnection Connection
        {
            get { return connection; }
        }

        public static BaseDB Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new BaseDB();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
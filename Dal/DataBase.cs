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
using SQLite;

namespace Dal
{
    public class DataBase
    {
        private static string dbName = "AppDB.db";
        private static SQLiteConnection connection;
        private static DataBase instance = null;

        // lock
        private static readonly object padlock = new object();

        private DataBase()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = System.IO.Path.Combine(documentsPath, dbName);

            connection = new SQLiteConnection(path);
        }

        public static string DbName
        {
            get { return dbName; }
            set { if (value != null) dbName = value; }
        }

        public SQLiteConnection Connection
        {
            get { return connection; }
        }

        public static DataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DataBase();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
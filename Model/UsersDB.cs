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
using Dal;

namespace Model
{
    public class UsersDB : BaseDB
    {
        private int affectedRows;

        public SQLite.CreateTableResult CreateTable()
        {
            result = connection.CreateTable<User>();
            return result;
        }

        public int Insert(User user)
        {
            CreateTable();
            affectedRows = connection.Insert(user);
            return affectedRows;
        }

        public int Update(User user)
        {
            affectedRows = connection.Update(user);
            return affectedRows;
        }

        public int Delete(User user)
        {
            affectedRows = connection.Delete(user);
            return affectedRows;
        }

        public int Delete(int id)
        {
            affectedRows = connection.Delete(id);
            return affectedRows;
        }

        public Users GetAll(bool sorted = true)
        {
            Users users = new Users();

            try
            {
                List<User> usersList = new List<User>();
                if (sorted)
                {
                    string sql = "SELECT * FROM Users ORDER BY ID";
                    usersList = connection.Query<User>(sql).ToList();
                }
                else
                {
                    usersList = connection.Table<User>().ToList();
                }

                foreach (User user in usersList)
                    users.Add(user);

            }
            catch (SQLite.SQLiteException e)
            {
                return users;
            }

            return users;
        }

        public User GetUser(int id)
        {
            return connection.Get<User>(id);
        }
    }
}
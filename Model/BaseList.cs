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

namespace Model
{
    [Serializable]
    public abstract class BaseList<T> : List<T> where T : new()
    {
        public List<T> InsertList { get; set; }
        public List<T> UpdateList { get; set; }
        public List<T> DeleteList { get; set; }

        /// <summary>
        /// <== רשימת האיברים שלא נמחקו
        /// EntityStatus = Deleted-כל האיברים פרט לאלה ש
        /// </summary>
        public List<T> UndeletedList
        {
            get { return (this == null) ? null : this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) != EntityStatus.DELETED).ToList(); }
        }

        #region ABSTRACT / VIRTUAL METHODS

        public abstract bool Exists(T t, bool forChange = false);
        public new abstract void Sort();

        /// <summary>
        /// שמירת הנתונים
        ///                                   מטודה זו נקראת לאחר ביצוע המטודה במחלקה היורשת
        ///      (הפעולות: 1. מחיקה מהרשימה של כל האיברים שנמחקו (לאחר המחיקה ממסד הנתונים
        /// (לאחר הוספה/עדכון במסד הנתונים) Unchanged-סימון כל האיברים ברשימה כ 
        /// </summary>
        public virtual bool Save()
        {
            RemoveAll(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.DELETED);
            ForEach(item => item.GetType().GetProperty("EntityStatus").SetValue(item, EntityStatus.UNCHANGED, null));

            return isUpdateOK;
        }

        protected bool isUpdateOK;

        /// <summary>
        /// Genereta InsertList, UpdateList, DeleteList
        /// </summary>
        protected void GenereteUpdateLists()
        {
            InsertList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.ADDED).ToList();
            UpdateList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.MODIFIED).ToList();
            DeleteList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.DELETED).ToList();
        }

        #endregion ABSTRACT / VIRTUAL METHODS
    }
}
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
    public abstract class BaseList<T> : List<T> where T : new()
    {
        public List<T> InsertList { get; set; }
        public List<T> UpdateList { get; set; }
        public List<T> DeleteList { get; set; }

        /*public List<T> UndeletedList
        {
            get { return (this == null) ? null : this.Where(item => (EntityStatus)item.Get()); }
        }*/

        public abstract bool Exists(T t, bool forChange = false);
        public new abstract void Sort();
    }
}
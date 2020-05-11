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

namespace Model
{
    public enum EntityStatus { ADDED, MODIFIED, DELETED, UNCHANGED }
    [Serializable]
    public abstract class BaseEntity
    {
        private int id;
        private EntityStatus entityStatus;

        protected BaseEntity() : this(EntityStatus.UNCHANGED) { }

        protected BaseEntity(EntityStatus entityStatus)
        {
            this.entityStatus = entityStatus;
        }
        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }

        [Ignore]
        public EntityStatus EntityStatus { get => entityStatus; set => entityStatus = value; }

        public override bool Equals(object obj)
        {
            return obj is BaseEntity entity &&
                   id == entity.id &&
                   entityStatus == entity.entityStatus;
        }
    }
}
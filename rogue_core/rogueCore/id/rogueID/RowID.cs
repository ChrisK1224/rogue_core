using System;
using System.Collections.Generic;

namespace rogue_core.rogueCore.id
{
    public abstract class RowID
    {
        protected readonly int _intVal;
        public long longVal { get; }
        public RowID(int val)
        {
            this._intVal = val;
            this.longVal = val;
        }
        public override int GetHashCode()
        {
            return _intVal;
        }
        public bool Equals(int obj)
        {
            return obj == this._intVal;
        }
        public override bool Equals(object obj)
        {
            // if (obj == null || GetType() != obj.GetType())
            //     return false;
            var other = (RowID)obj;
            return this._intVal == other._intVal;
        }
        public override String ToString()
        {
            return _intVal.ToString();
        }
        public int ToInt()
        {
            return _intVal;
        }
    }
}
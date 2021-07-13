using System;

namespace rogue_core.rogueCore.id.rogueID
{
    public abstract class JustAnInt
    {
        protected readonly int _intVal;
        public static implicit operator int(JustAnInt id)
        {
            return id._intVal;
        }
        
        public JustAnInt(int val)
        {
            this._intVal = val;
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
            return this.GetHashCode() == obj.GetHashCode();
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
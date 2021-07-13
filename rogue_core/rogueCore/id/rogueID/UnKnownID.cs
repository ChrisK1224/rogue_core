using System;

namespace rogue_core.rogueCore.id.rogueID
{
    public class UnKnownID : RowID
    {
        public UnKnownID(int id) : base(id){ }
        public UnKnownID(String id) : base(int.Parse(id)){}
        public static implicit operator int(UnKnownID id)
        {
            return id._intVal;
        }
        public static implicit operator UnKnownID(int id)
        {
            return new UnKnownID(id);
        }
    }
}
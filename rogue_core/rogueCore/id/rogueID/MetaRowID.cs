namespace rogue_core.rogueCore.id.rogueID
{
    public abstract class MetaRowID : RowID
    {
        public MetaRowID(int val) : base(val){}

        // public static implicit operator int(MetaRowID id)
        // {
        //     return id._intVal;
        // }
        // public static implicit operator MetaRowID(int id)
        // {
        //     return new MetaRowID(id);
        // }
    }
}
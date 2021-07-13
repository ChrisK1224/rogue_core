namespace rogue_core.rogueCore.id.rogueID.decodedIDs
{
    public class VarcharRowID : DecodedRowID
    {
        public VarcharRowID(int id) : base(id){ }
        public static implicit operator int(VarcharRowID id)
        {
            return id._intVal;
        }
        public static implicit operator VarcharRowID(int id)
        {
            return new VarcharRowID(id);
        }
    }
}
namespace rogue_core.rogueCore.id.rogueID.decodedIDs
{
    public class NumberRowID : DecodedRowID
    {
        public NumberRowID(int id) : base(id){ }
        public static implicit operator int(NumberRowID id)
        {
            return id._intVal;
        }
        public static implicit operator NumberRowID(int id)
        {
            return new NumberRowID(id);
        }
    }
}
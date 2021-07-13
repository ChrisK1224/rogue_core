namespace rogue_core.rogueCore.id.rogueID.decodedIDs
{
    public class DateRowID : DecodedRowID
    {
        public DateRowID(int id) : base(id){ }
        public static implicit operator int(DateRowID id)
        {
            return id._intVal;
        }
        public static implicit operator DateRowID(int id)
        {
            return new DateRowID(id);
        }
    }
}
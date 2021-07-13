namespace rogue_core.rogueCore.id.rogueID
{
    public class DataRowID : RowID
    {
        public DataRowID(int id) : base(id){ }
        public static implicit operator int(DataRowID id)
        {
            return id._intVal;
        }
        public static implicit operator DataRowID(int id)
        {
            return new DataRowID(id);
     
        }
    }
}
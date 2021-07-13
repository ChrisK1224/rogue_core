namespace rogue_core.rogueCore.id.rogueID.hqlIDs
{
    public class ComboRowID : JustAnInt
    {
        public ComboRowID(int val) : base(val){}
        public static implicit operator ComboRowID(int id)
        {
            return new ComboRowID(id);
        }
        public static implicit operator int(ComboRowID id)
        {
            return id._intVal;
        }
    }
}
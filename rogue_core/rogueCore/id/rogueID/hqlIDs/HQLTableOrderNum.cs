namespace rogue_core.rogueCore.id.rogueID.hqlIDs
{
    public class @int : JustAnInt
    {
        public @int(int val) : base(val){}
        public static implicit operator @int(int id)
        {
            return new @int(id);
        }
        public static implicit operator int(@int id)
        {
            return id._intVal;
        }
    }
}
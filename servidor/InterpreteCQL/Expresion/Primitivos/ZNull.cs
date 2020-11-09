namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZNull:ZContenedor
    {
        public ZNull(ZContenedor anterior, TeDeU origen) : base(anterior, origen)
        {
        }

        public override string stringBonito()
        {
            return "null";
        }
        
    }
}
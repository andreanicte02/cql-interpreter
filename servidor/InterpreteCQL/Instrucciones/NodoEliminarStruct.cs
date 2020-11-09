using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoEliminarStruct: Instruccion
    {

        private readonly string nombreStruct;

        public NodoEliminarStruct(string nombreStruct)
        {
            this.nombreStruct = nombreStruct;
        }


        public override object ejecutarSinposicion(ZContenedor e)
        {
            e.eliminarTeDeU(nombreStruct);
            return null;
        }
    }
}
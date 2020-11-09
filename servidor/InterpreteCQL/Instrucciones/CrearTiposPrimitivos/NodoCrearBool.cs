using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearBool:Instruccion
    {
        private bool val;

        public NodoCrearBool(bool val)
        {
            this.val = val;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new ZBool(val);
        }
    }
}
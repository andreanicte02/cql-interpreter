using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearNulo: Instruccion
    {
        public override object ejecutarSinposicion(ZContenedor e)
        {
           // return TiposPrimitivos.instanciaNulo;
           return TiposPrimitivos.instanicaNulo;
        }
    }
}
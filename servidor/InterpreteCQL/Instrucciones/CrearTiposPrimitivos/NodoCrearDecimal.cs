using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearDecimal:Instruccion
    {
        private readonly double valor;


        public NodoCrearDecimal(double valor) : base()
        {
            this.valor = valor;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new ZDecimal(valor);
        }
    }
}
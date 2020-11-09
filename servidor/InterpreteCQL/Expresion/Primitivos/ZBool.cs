using System;
using servidor.InterpreteCQL.Instruccionesn;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZBool: ZContenedor
    {
        private readonly bool valor;

        public ZBool(bool valor):base(null,TiposPrimitivos.tipoBool)
        {
            this.valor = valor;
        }
        
        public bool obtenerValor() {

            return valor;
        }
        
        public override String stringBonito() {

            return  valor+"";

        }
        
        
    }
}
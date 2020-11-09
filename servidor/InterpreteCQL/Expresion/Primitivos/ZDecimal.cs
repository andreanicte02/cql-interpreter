using System;
using servidor.InterpreteCQL.Instruccionesn;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZDecimal:ZContenedor
    {
        private double valor;
        public ZDecimal(double valor) : base(null, TiposPrimitivos.tipoDicimal)
        {
            this.valor = valor;
        }
        
        public double obtenerValor() {

            return valor;
        }
        
        public override string stringBonito()
        {
            return valor + "";
        }

        public void definirValor(double valor)
        {
            this.valor = valor;
        }

    }
}
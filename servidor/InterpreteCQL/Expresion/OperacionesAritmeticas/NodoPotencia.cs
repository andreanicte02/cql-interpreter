using System;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesAritmeticas
{
    public class NodoPotencia:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoPotencia(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }


        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);
            var algo2 = i2.ejecutar(e);

            ZContenedor valor1 = Utilidades.desenvolver(algo1);
            ZContenedor valor2 = Utilidades.desenvolver(algo2);

            if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
            {
                return new ZDecimal(Math.Pow(n1.obtenerValor() , n2.obtenerValor()));
                
            }
            if (valor1 is ZDecimal n11 && valor2 is ZDecimal n12)
            {
                return new ZDecimal(Math.Pow(n11.obtenerValor() , n12.obtenerValor()));
                
            }
            if (valor1 is ZNumero n111 && valor2 is ZDecimal n122)
            {
                return new ZDecimal(Math.Pow(n111.obtenerValor() , n122.obtenerValor()));
                
            }
            if (valor1 is ZDecimal n11a && valor2 is ZNumero n12b)
            {
                return new ZDecimal(Math.Pow(n11a.obtenerValor(), n12b.obtenerValor()));
                
            }

            throw new SemanticError("Error; operacion ** no compatible con tipos");
        }
    }
}
using System;
using System.Linq;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesAritmeticas
{
    public class NodoSuma: Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;
        
        public NodoSuma(Instruccion i1,Instruccion i2) : base()
        {
            this.i1 = i1;
            this.i2 = i2;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);
            var algo2 = i2.ejecutar(e);

            var valor1 = Utilidades.desenvolver(algo1);
            var valor2 = Utilidades.desenvolver(algo2);

            if (valor1 is ZLista zLista1 && valor2 is ZLista zLista2 && zLista1.Origen == zLista2.Origen)
            {
                return ZLista.crearLista(zLista1.Elementos.Concat(zLista2.Elementos).ToList());
            }

            
            if (valor1 is ZCadena c1 && valor2 is ZCadena c2){
                
                return  new ZCadena(c1.obtenerValor() + c2.obtenerValor());
            }
            
            if (valor1 is ZCadena c3 && valor2 is ZNumero n1 ){
                
                return  new ZCadena(c3.obtenerValor() +n1.obtenerValor() );
            }

            if (valor1 is ZNumero n2 && valor2 is ZCadena c4){
                
                return  new ZCadena(n2.obtenerValor() +c4.obtenerValor() );

            }
            
            if (valor1 is ZCadena c5 && valor2 is ZBool b1){
                
                return  new ZCadena(c5.obtenerValor() +b1.obtenerValor() );

            }

            if(valor1 is ZBool b2 && valor2 is ZCadena c6){
                
                return  new ZCadena(b2.obtenerValor() +c6.obtenerValor() );

            }

            if (valor1 is ZCadena c7 && valor2 is ZDecimal d1)
            {
                return  new ZCadena(c7.obtenerValor() +d1.obtenerValor() );

            }

            if (valor1 is ZDecimal d3 && valor2 is ZCadena c8){
                
                return  new ZCadena(d3.obtenerValor() +c8.obtenerValor() );

            }


            if (valor1 is ZNumero n3 && valor2 is ZNumero n4)
            {
                return new ZNumero(n3.obtenerValor() + n4.obtenerValor());
                
            }
            if (valor1 is ZDecimal n5 && valor2 is ZDecimal n6)
            {
                return new ZDecimal(n5.obtenerValor() + n6.obtenerValor());
                
            }
            if (valor1 is ZNumero n7 && valor2 is ZDecimal d2)
            {
                return new ZDecimal(n7.obtenerValor() + d2.obtenerValor());
                
            }
            if (valor1 is ZDecimal n8 && valor2 is ZNumero n9)
            {
                return new ZDecimal(n8.obtenerValor() + n9.obtenerValor());
                
            }

            if ((valor1 is ZDate|| valor1 is ZTiempo) && valor2 is ZCadena n11)
            {
                return new ZCadena(valor1.stringBonito() + n11.obtenerValor());
                
            }

            if (valor1 is ZCadena n12 && (valor2 is ZDate || valor2 is ZTiempo))
            {
                return new ZCadena(n12.obtenerValor() + valor2.stringBonito());
            }


            throw new SemanticError("Error; operacion + no compatible con tipos");

        }
    }
}
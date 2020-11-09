using System;
using Irony.Parsing;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Sentencias
{
    public class NodoCasteoExplicito:Instruccion
    {
        private Instruccion tipo;
        private Instruccion valor;

        public NodoCasteoExplicito(Instruccion tipo, Instruccion valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU tedeu = (TeDeU)tipo.ejecutar(e);
            Object algo = this.valor.ejecutar(e);
            ZContenedor valor = Utilidades.desenvolver(algo);


            
            //lado izquierdo cadena
            if (tedeu== TiposPrimitivos.tipoString)
            {
                return casteoExplicitoCadnea(valor);
            }

            //lado derecho cadena
            if (valor is ZCadena n1)
            {
                return ladoDerechoCadena(n1.obtenerValor(), tedeu);
            }
            
            
            
            throw new SemanticError("se esta intentando castear un tipo a string no compatible");

        }

        public ZContenedor  casteoExplicitoCadnea(ZContenedor valor)
        {
            if (valor is ZDate n1)
            {
                return  new ZCadena(n1.stringBonito());
            }

            if (valor is ZTiempo n2)
            {
                return  new ZCadena(n2.stringBonito());
            }

            if (valor is ZNumero n3)
            {
                return  new ZCadena(n3.obtenerValor() +"");
            }

            if (valor is ZDecimal n4)
            {
                return  new ZCadena(n4.obtenerValor() +"");
            }

            if (valor is ZCadena)
            {
                return valor;
            }

            throw new SemanticError("se esta intentando castear un tipo a string no compatible");
        }

        public ZContenedor ladoDerechoCadena(string valor , TeDeU tipo)
        {
            if (tipo == TiposPrimitivos.tipoDate)
            {
                return new ZDate(valor);
            }

            if (tipo == TiposPrimitivos.tipoTime)
            {
                return  new ZTiempo(valor);
            }

            if (tipo == TiposPrimitivos.tipoNumero)
            {
                try
                {
                    return  new ZNumero(Convert.ToInt32(valor));
                }
                catch (Exception e)
                {
                    throw new SemanticError("no se puede convertir este valor a entero");
                }
            }

            if (tipo == TiposPrimitivos.tipoDicimal)
            {
                try
                {
                    return  new ZDecimal(Convert.ToDouble(valor));
                }
                catch (Exception e)
                {
                   throw new SemanticError("no se puede convertir este valor a decimal");
                }
               
            }
            throw new SemanticError("se esta intentando castear un tipo a string no compatible");


        }
    }
}
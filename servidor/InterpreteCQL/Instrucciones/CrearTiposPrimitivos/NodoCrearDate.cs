using System;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearDate: Instruccion
    {
        private string valor;

        public NodoCrearDate(string valor)
        {
            
                this.valor = valor;
            
           
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

            return new ZDate(valor);
        }
    }
}
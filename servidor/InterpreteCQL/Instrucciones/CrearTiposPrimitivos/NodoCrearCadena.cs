using System;
using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearCadena:Instruccion
    {
        private readonly String valor;

        public NodoCrearCadena(string valor)
        {
            this.valor = valor;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new ZCadena(valor);
        }
    }
}
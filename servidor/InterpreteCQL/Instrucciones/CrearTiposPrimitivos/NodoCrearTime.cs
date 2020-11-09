using System;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearTime:Instruccion
    {
        private string valor;

        public NodoCrearTime(string valor)
        {
            
                this.valor =valor;
            
            
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new ZTiempo(valor);
        }
    }
}
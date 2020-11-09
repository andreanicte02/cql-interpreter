using System;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoCrearNumero: Instruccion
    {
        private readonly int valor;

        public NodoCrearNumero(int valor) : base()
        {
            this.valor = valor;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new ZNumero(valor);
        }

    
    }
}
using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoParametro:Instruccion
    {
        private List<NodoCrearParametro> lista;

        public NodoParametro(List<NodoCrearParametro> lista)
        {
            this.lista = lista;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return lista;
        }
    }
}
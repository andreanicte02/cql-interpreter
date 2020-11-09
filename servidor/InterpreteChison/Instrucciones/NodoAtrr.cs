using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoAtrr:Instruccion
    {
        private List<NodoDeclararParametro> _parametros;
        private string nombre;


        public NodoAtrr(List<NodoDeclararParametro> parametros)
        {
            _parametros = parametros;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            NodoDeclararStruct nodo = new NodoDeclararStruct(nombre, _parametros);
            return nodo.ejecutar(e);
        }

        public void definirNombre(string nombre)
        {
            this.nombre = nombre;
        }
    }
}
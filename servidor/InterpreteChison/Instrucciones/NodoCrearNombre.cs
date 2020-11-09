using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoCrearNombre:Instruccion
    {
        private string nombre;

        public NodoCrearNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return nombre;
        }
    }
}
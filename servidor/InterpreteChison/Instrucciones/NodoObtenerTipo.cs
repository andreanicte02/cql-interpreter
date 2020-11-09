using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoObtenerTipo: Instruccion
    {
        private string tipo;
        
        public NodoObtenerTipo(string tipo)
        {
            this.tipo = tipo;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return tipo;
        }
    }
}
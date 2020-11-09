using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoCrearInstancia:Instruccion
    {
        private string id;

        public NodoCrearInstancia(string id)
        {
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU tedeu = e.getTeDeU(id);
            return tedeu.crearInstancia(e);
        }
    }
}
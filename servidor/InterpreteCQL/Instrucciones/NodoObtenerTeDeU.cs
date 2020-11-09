using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoObtenerTeDeU:Instruccion
    {
        private readonly string nombreTipo;

        public NodoObtenerTeDeU(string nombreTipo)
        {
            this.nombreTipo = nombreTipo;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU t= e.getTeDeU(nombreTipo); //obtner tipo tdu //asignacion //
            
            return t;
        }
        
        
        
        
    }
}
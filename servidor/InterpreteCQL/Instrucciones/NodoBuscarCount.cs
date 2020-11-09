using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoBuscarCount:Instruccion
    {
        

        public override object ejecutarSinposicion(ZContenedor e)
        {

            TeDeU tipo = e.getTeDeU("int");
            tipo.isCount = true;
            return tipo;
        }
    }
}
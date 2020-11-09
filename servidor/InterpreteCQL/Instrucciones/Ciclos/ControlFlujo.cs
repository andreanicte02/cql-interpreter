using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Ciclos
{
    public class ControlFlujo
    {
        
    }

    public class NodoBreak:Instruccion
    {
        public override object ejecutarSinposicion(ZContenedor e)
        {
            return this;
        }
    }

    public class NodoContinue:Instruccion
    {
        public override object ejecutarSinposicion(ZContenedor e)
        {
            return this;
        }
    }
    
}
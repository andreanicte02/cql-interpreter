using System.Collections.Generic;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    
    public class NodoInvocarSoloFuncion:Instruccion
    {
        private string id;
        private List<Instruccion> InstruccionesArgumentos;


        public NodoInvocarSoloFuncion(string id, List<Instruccion> instruccionesArgumentos)
        {
            this.id = id;
            InstruccionesArgumentos = instruccionesArgumentos;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            List<ZContenedor> argumentos = Utilidades.desnvolverArgumento(InstruccionesArgumentos, e);
            return Utilidades.invocarSoloFuncion(e, id, argumentos);


        }
    }
}
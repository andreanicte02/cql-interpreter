using System.Collections.Generic;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoInvocarProcedure:Instruccion
    {
        private string id;
        private List<Instruccion> parametros;

        public NodoInvocarProcedure(string id, List<Instruccion> parametros)
        {
            this.id = id;
            this.parametros = parametros;
        }


        public override object ejecutarSinposicion(ZContenedor e)
        {
            List<ZContenedor> argumentos = Utilidades.desnvolverArgumento(parametros,e);
            return Utilidades.invocarProcedimiento(e,id,argumentos);
        }
    }
}
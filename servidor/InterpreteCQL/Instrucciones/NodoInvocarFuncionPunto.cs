using System.Collections.Generic;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoInvocarFuncionPunto:Instruccion
    {
        private Instruccion expresion;
        private string id;
        private List<Instruccion> argumentos;

        public NodoInvocarFuncionPunto(Instruccion expresion, string id, List<Instruccion> argumentos)
        {
            this.expresion = expresion;
            this.id = id;
            this.argumentos = argumentos;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo = expresion.ejecutar(e);
            ZContenedor valor = Utilidades.desenvolver(algo);
            List<ZContenedor> argumentos = Utilidades.desnvolverArgumento(this.argumentos,e);
            return Utilidades.invocarFuncionPunto(valor, id, argumentos);
            
        }
    }
}
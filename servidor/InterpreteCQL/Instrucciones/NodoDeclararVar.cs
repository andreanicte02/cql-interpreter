using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoDeclararVar:Instruccion
    {
        private string id;
        private Instruccion expresion;
        private TeDeU tipo;

        public NodoDeclararVar(string id, Instruccion expresion)
        {
            this.id = id;
            this.expresion = expresion;
        }


        public override object ejecutarSinposicion(ZContenedor e)
        {
            Simbolo sim = new Simbolo(tipo, null);
            Utilidades.AsignarValorInicial(sim);
            
            if (expresion == null)
            {
                e.setVariable(id, sim);
                return null;
            }
            
            var algo = expresion.ejecutar(e);
            Utilidades.asginar(sim, Utilidades.desenvolver(algo));
            e.setVariable(id, sim);
            
            return null;

        }


        public void definirTipo(TeDeU tipo)
        {
            this.tipo = tipo;
        }
    }
}
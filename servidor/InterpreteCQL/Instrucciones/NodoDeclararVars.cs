using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoDeclararVars:Instruccion
    {
        private Instruccion tipo;
        private List<NodoDeclararVar> declaraciones;


        public NodoDeclararVars(Instruccion tipo, List<NodoDeclararVar> declaraciones)
        {
            this.tipo = tipo;
            this.declaraciones = declaraciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            
            

            //obtener tipo
            TeDeU t = (TeDeU) tipo.ejecutar(e);

            
            foreach (NodoDeclararVar decV in declaraciones)
            {
                decV.definirTipo(t);
                decV.ejecutar(e);
            }
            
            //e.setVariable(id,new Simbolo(t, null));
            
            return null;

        }
        
     
    }
}
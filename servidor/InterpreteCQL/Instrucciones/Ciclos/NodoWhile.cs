using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Ciclos
{
    public class NodoWhile:Instruccion
    {
        private Instruccion exp;
        private List<Instruccion> lInstrucciones;

        public NodoWhile(Instruccion exp, List<Instruccion> lInstrucciones)
        {
            this.exp = exp;
            this.lInstrucciones = lInstrucciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            
            while (Utilidades.evaluarCondicion(exp,e).obtenerValor())
            {
                ZContenedor local = new ZContenedor(e, null);
                local.enFuncion = e.enFuncion;
                local.enProcedimiento = e.enProcedimiento;
                var result = Utilidades.ejecutarSentencias(lInstrucciones, local);
               
                if (result is NodoBreak) {
            
                    return null;
                } 
                
                if (result is NodoContinue) {

                    continue;
                    
                }if (result is Retorno || result is RetornoProc)
                {
                    return result;
                }

              
            }
            return null;
        }

       
    }
}
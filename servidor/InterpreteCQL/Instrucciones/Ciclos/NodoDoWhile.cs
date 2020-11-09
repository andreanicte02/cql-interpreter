using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Ciclos
{
    public class NodoDoWhile:Instruccion
    {
        private readonly Instruccion exp;
        private readonly List<Instruccion> lLinstrucciones;

        public NodoDoWhile(Instruccion exp, List<Instruccion> lLinstrucciones)
        {
            this.exp = exp;
            this.lLinstrucciones = lLinstrucciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

            do
            {
                ZContenedor local = new ZContenedor(e, null);
                local.enFuncion = e.enFuncion;
                local.enProcedimiento = e.enProcedimiento;

                var result = Utilidades.ejecutarSentencias(lLinstrucciones, local);


                if (result is NodoBreak) {
            
                    return null;
                } 
                
                if (result is NodoContinue) {

                    continue;
                    
                }

                if (result is Retorno || result is RetornoProc)
                {
                    return result;
                }

            } while (Utilidades.evaluarCondicion(exp, e).obtenerValor());

            return null;
        }
    }
}
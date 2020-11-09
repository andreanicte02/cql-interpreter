using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Ciclos
{
    public class NodoFor:Instruccion
    {
        private readonly Instruccion inicicializacion;
        private readonly Instruccion condicion;
        private readonly Instruccion actualizacion;
        private readonly List<Instruccion> instrucciones;
        //for init -> exp = exp
        // tipo id = exp;
        
        //exp--> retornar booleando
        
        //actualizacion ---> exp = exp
        // exp ++
        // exp --
        // exp += exp


        public NodoFor(Instruccion inicicializacion, Instruccion condicion, Instruccion actualizacion, List<Instruccion> instrucciones)
        {
            this.inicicializacion = inicicializacion;
            this.condicion = condicion;
            this.actualizacion = actualizacion;
            this.instrucciones = instrucciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {


            //for (int x = 0; x < 5; x++) -> crea ambito -> padre
            //{
            //----> crear ambito //-hijo
            //}

            //--primer ambito
            ZContenedor local = new ZContenedor(e, null);
            local.enFuncion = e.enFuncion;
            local.enProcedimiento = e.enProcedimiento;

            //----> ejecutar incializacion
           inicicializacion.ejecutar(local);
           
           //--> evualuacr condicion-> primer ambito
           while (Utilidades.evaluarCondicion(condicion,local).obtenerValor())
           {
               
               ZContenedor local2 = new ZContenedor(local, null);
               local2.enFuncion = local.enFuncion;
               local2.enProcedimiento = local.enProcedimiento;

               var result = Utilidades.ejecutarSentencias(instrucciones, local2);
               
               if (result is NodoBreak) {

                   return  null;
               } 
                
               if (result is NodoContinue) {

                   actualizacion.ejecutar(local);
                   continue;
                    
               }

               if (result is Retorno || result is RetornoProc)
               {
                   return result;
               }


               actualizacion.ejecutar(local);
               // x++
               // x = x +1


           }

           return null;
        }
    }
}
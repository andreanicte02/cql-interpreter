using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using servidor.InterpreteCQL.Expresion.OperacionesRelacionales;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones.Ciclos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Sentencias
{
    public class NodoSwitch:Instruccion
    {
        private readonly Instruccion exp;
        private readonly List<NodoCase> cases;
        private readonly Instruccion @defult;

        public NodoSwitch(Instruccion exp, List<NodoCase> cases, Instruccion defult)
        {
            this.exp = exp;
            this.cases = cases;
            this.defult = defult;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            //switch (2)
            //{
                    
                // case 3:
                //case 2 : instruciones
                //.
                //defualt: 
            //}
            ZContenedor local = new ZContenedor(e,null); //crear el entorno
            local.enFuncion = e.enFuncion;
            local.enProcedimiento = e.enProcedimiento;
          
            foreach (NodoCase @case in cases)
            {
          
                //entorno local por caso
               
                NodoIgualQue aux = new NodoIgualQue(exp, @case.Exp);
                ZBool condicion = Utilidades.evaluarCondicion(aux, e); //--- boleando return

                
                if (condicion.obtenerValor())
                {
                    var result = @case.ejecutar(local);
                  
                    if (result is NodoBreak) {
                    
                        return null;
                    }

                    if (result is Retorno || result is RetornoProc)
                    {
                        return result;
                    }

                }

            }
            
            var resultDefault = defult.ejecutar(local);

            if (resultDefault is NodoBreak) {
                    
                return null;
            }

            if (resultDefault is Retorno || resultDefault is RetornoProc)
            {
                return resultDefault;
            }
            // si hay un continue se ignora

            return null;
        }
    }

    public class NodoCase : Instruccion
    { 
        Instruccion exp;
        private readonly List<Instruccion> lInstructions;

        //case exp:
        public NodoCase(Instruccion exp, List<Instruccion> lInstructions)
        {
            this.exp = exp;
            this.lInstructions = lInstructions;
        }

        public Instruccion Exp
        {
            get => exp;
            set => exp = value;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {

            return Utilidades.ejecutarSentencias(lInstructions, e);

        }
    }

    public class NodoDefault: Instruccion
    {
        public NodoDefault(List<Instruccion> lInstructions)
        {
            this.lInstructions = lInstructions;
        }

        private readonly List<Instruccion> lInstructions;
        public override object ejecutarSinposicion(ZContenedor e)
        {
            //new list<Instruccion>
            return Utilidades.ejecutarSentencias(lInstructions, e);
        }
    }
}
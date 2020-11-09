using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Sentencias
{
    public class NodoIf:Instruccion
    {
        private readonly Instruccion exp;
        private List<Instruccion> trueInstructions;

        public List<Instruccion> TrueInstructions
        {
            get => trueInstructions;
            set => trueInstructions = value;
        }

        public List<Instruccion> FalseInstructions
        {
            get => falseInstructions;
            set => falseInstructions = value;
        }

        private List<Instruccion> falseInstructions; 

        public NodoIf(Instruccion exp, List<Instruccion> trueInstructions, List<Instruccion> falseInstructions)
        {
            this.exp = exp;
            this.trueInstructions = trueInstructions;
            this.falseInstructions = falseInstructions;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

            ZBool condicion = Utilidades.evaluarCondicion(exp, e);
            ZContenedor local = new ZContenedor(e,null);
            local.enFuncion = e.enFuncion;
            local.enProcedimiento = e.enProcedimiento;
            
            List<Instruccion> sentencias =
                condicion.obtenerValor() ? trueInstructions : falseInstructions;
            
            return Utilidades.ejecutarSentencias(sentencias, local);
        }
    }
}
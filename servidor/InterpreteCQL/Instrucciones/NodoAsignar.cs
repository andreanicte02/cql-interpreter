
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoAsignar:Instruccion
    {
        private  readonly Instruccion exp1;

        private  readonly Instruccion exp2;

        public Instruccion Exp2 => exp2;

        public Instruccion Exp1 => exp1;

        public NodoAsignar(Instruccion exp1, Instruccion exp2)
        {
            this.exp1 = exp1;
            this.exp2 = exp2;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = exp1.ejecutar(e);
            var algo2 = exp2.ejecutar(e);

            
            
            if (!(algo1 is Simbolo))
            {
                throw new SemanticError("Lado izquierdo no es una referencia");
            }

            Utilidades.asginar( (Simbolo)algo1 , algo2);

            return null;
        }

  

      
    }
}
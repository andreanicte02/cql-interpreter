using System;
using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoFila:Instruccion
    {
        private List<NodoAsignar> filas;


        public NodoFila(List<NodoAsignar> filas)
        {
            this.filas = filas;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            foreach (NodoAsignar nodo in filas)
            {
                if (nodo.Exp2 is NodoFila n2)
                {
                    Simbolo algo = (Simbolo)nodo.Exp1.ejecutar(e);
                    ZInstancia ins = algo.obtenerInstanciaTipo().crearInstancia(e);
                    n2.ejecutar(ins);
                    algo.definirValor(ins);

                }

                if (!(nodo.Exp2 is NodoFila))
                {
                    nodo.ejecutar(e);
                }


                
            }

            return null;
        }
    }
}